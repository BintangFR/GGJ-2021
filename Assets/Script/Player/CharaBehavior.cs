using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBehavior : MonoBehaviour
{
    [SerializeField] protected CharaData data;
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected float moonJump;
    [SerializeField] protected bool doubleJump;
    [SerializeField] protected bool canJump;
    [SerializeField] protected Vector2 direction;
    [SerializeField] protected LayerMask ground;
    [SerializeField] protected float groundLength;
    [SerializeField] public float linearDrag = 4f;
    [SerializeField] public float gravity = 1f;
    [SerializeField] public float fallMultiplier = 5f;
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
            anim.SetBool("Walk", true);
        }
        ChangeFlip();
    }

    public void Jump(bool flag)
    {
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
            anim.SetTrigger("Jump");
        }
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
            sprite.flipX = false;
        }
        else
        {
            sprite.flipX = true;
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
        }
    }

}
