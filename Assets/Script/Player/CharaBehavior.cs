using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaBehavior : MonoBehaviour
{
    [SerializeField] protected float speed;
    [SerializeField] protected float jumpSpeed;
    [SerializeField] protected Vector2 direction;
    [SerializeField] protected LayerMask ground;
    private Rigidbody2D rb;
    private SpriteRenderer sprite;

    public void Init()
    {
        rb = GetComponent<Rigidbody2D>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Move()
    {
        rb.velocity = direction;
        ChangeFlip();
    }

    public void Jump()
    {
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
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
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 1f, ground);
        Debug.DrawRay(transform.position, Vector2.down * 1f, Color.red); // Untuk menngetahui seberapa panjang RaycastHit2D
        if (hit)
        {
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
