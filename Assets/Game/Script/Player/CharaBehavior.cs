using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TomWill;

public class CharaBehavior : MonoBehaviour
{
    [Header ("Class Stuff")] 
    [SerializeField] protected CharaData data;

    [Header ("Player Stuff")]
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected float moonJump;
    [SerializeField] protected float jumpDelay;
    [SerializeField] protected bool doubleJump;
    [SerializeField] protected bool canJump;
    [SerializeField] protected bool canGliding;
    [HideInInspector] protected bool isDoubleJump, isGliding;
    [SerializeField] protected float fallSpeed;
    [SerializeField] protected Vector2 direction;

    [Header ("Raycast")] 
    [SerializeField] protected LayerMask ground;
    [SerializeField] protected float groundLength;

    [Header ("Physics2D")]
    [SerializeField] public float linearDrag = 4f;
    [SerializeField] public float gravity = 1f;
    [SerializeField] public float fallMultiplier = 5f;

    [Header ("Particle System")]
    [SerializeField] protected ParticleSystem walkParticle;
    [SerializeField] protected ParticleSystem jumpParticle;
    [SerializeField] protected ParticleSystem featherParticle;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;
    private Animator anim;

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        speed = data.BaseSpeed;
        jumpSpeed = data.BaseJumpSpeed;
        moonJump = data.BaseMoonJump;
        jumpDelay = data.JumpDelay;
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move()
    {
        if (CheckGround())
        {
            rb.velocity = direction * speed;
            if (!walkParticle.isPlaying) walkParticle.Play();
            anim.SetBool("Walk", true);
            TWAudioController.PlaySFX("PLAYER_SFX", "player_walk");
        }
        ChangeFlip();
    }

    public void Jump(bool flag)
    {
        walkParticle.Stop();

        if (!flag)
        {
            rb.velocity = new Vector2(rb.velocity.x * moonJump, 0);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            anim.SetTrigger("Jump");
        }
        else
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            canJump = false;
            isDoubleJump = true;
            anim.SetTrigger("Jump");
        }
    }

    public void Glide()
    {
        rb.velocity = new Vector2(rb.velocity.x, Mathf.Sign(rb.velocity.y) * fallSpeed);
        isGliding = true;
        Debug.Log("Glide");
    }

    public void ModifyPhysics()
    {
        bool changingDirections = (direction.x > 0 && rb.velocity.x < 0) || (direction.x < 0 && rb.velocity.x > 0);

        if (CheckGround())
        {
            if (Mathf.Abs(direction.x) < 0.4f || changingDirections)
            {
                rb.drag = linearDrag;
            }
            else
            {
                rb.drag = 0f;
            }
            rb.gravityScale = 0;
        }
        else
        {
            rb.gravityScale = gravity;
            rb.drag = linearDrag * 0.15f;
            if (rb.velocity.y < 0)
            {
                rb.gravityScale = gravity * fallMultiplier;
                doubleJump = true;
                canGliding = true;
            }
            else if (rb.velocity.y > 0 && !Input.GetKeyDown(KeyCode.Space))
            {
                rb.gravityScale = gravity * (fallMultiplier / 2);
            }
        }
    }

    public void ChangeFlip()
    {
        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
            //sprite.flipX = false;
        }
        else
        {
            transform.localScale = new Vector3(-1, 1, 1);
            //sprite.flipX = true;
        }
    }

    public bool CheckGround()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, groundLength, ground);
        Debug.DrawRay(transform.position, Vector2.down * groundLength, Color.red); // Untuk menngetahui seberapa panjang RaycastHit2D
        if (hit)
        {
            anim.SetTrigger("Landing");
            doubleJump = false;
            isDoubleJump = false;
            isGliding = false;
            canGliding = false;
            canJump = true;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            direction.x *= -1;
            featherParticle.Play();
            CameraShake.instance.Shake(2, 1, 2);
        }

        if (collision.gameObject.CompareTag("Collectible"))
        {
            GameData.instance.ChickCollect++;
            Debug.Log(GameData.instance.ChickCollect);
            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Obstacle"))
        {
            this.gameObject.SetActive(false);
            GameVariables.GAME_OVER = true;
            TWAudioController.PlaySFX("PLAYER_SFX", "player_saw_death");
            InGameUI.instance.ShowLoseMenu();
        }
    }

}
