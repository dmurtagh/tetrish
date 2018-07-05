using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float maxSpeed = 10f;
    public float jumpForce = 700f;

    bool facingRight = true;
    Rigidbody2D rigidbody2d;
    Animator animator;

    // For ground collisions
    public LayerMask whatIsGround;
    public Transform groundCheck;
    bool grounded = false;
    float groundRadius = 0.2f;

    bool doubleJump = false;

    // Use this for initialization
    void Start ()
    {
        rigidbody2d = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        animator = GetComponent(typeof(Animator)) as Animator;
	}

    private void Update()
    {
        // ToDo: Need to use Input manager to map keys to jump
        if ((grounded || !doubleJump) && Input.GetKeyDown(KeyCode.Space))
        {
            animator.SetBool("Ground", false);
            rigidbody2d.AddForce(new Vector2(0, jumpForce));

            if (!doubleJump && !grounded)
            {
                doubleJump = true;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        animator.SetBool("Ground", grounded);

        if (grounded)
        {
            doubleJump = false;
        }

        animator.SetFloat("vSpeed", rigidbody2d.velocity.y);

        float move = Input.GetAxis("Horizontal");
        animator.SetFloat("Speed", Mathf.Abs(move));
        rigidbody2d.velocity = new Vector2(move * maxSpeed, rigidbody2d.velocity.y);

        if (move > 0 && !facingRight)
        {
            flip();
        }
        else if (move < 0 && facingRight)
        {
            flip();
        }
	}

    void flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
