using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float m_MaxSpeed = 10f;
    public float m_JumpForce = 700f;

    // For movement
    Rigidbody2D m_Rigidbody2d;
    bool m_FacingRight = true;

    // For animation
    Animator m_Animator;

    // For ground collisions
    public LayerMask m_WhatIsGround;
    public Transform m_GroundCheck;
    bool m_Grounded = false;
    float m_GroundRadius = 0.2f;

    // For jumping
    bool m_DoubleJump = false;

    // Use this for initialization
    void Start ()
    {
        m_Rigidbody2d = GetComponent(typeof(Rigidbody2D)) as Rigidbody2D;
        m_Animator = GetComponent(typeof(Animator)) as Animator;
	}

    private void Update()
    {
        CheckForJump();
        CheckForReset();
    }

    private void CheckForReset()
    {
        if (InputManager.Instance.GetResetKeyDown())
        {
            var resettables = FindObjectsOfType<AResettable>();
            foreach (var resettable in resettables)
            {
                resettable.Reset();
            }
        }
    }

    private void CheckForJump()
    {
        if ((m_Grounded || !m_DoubleJump) && 
            InputManager.Instance.GetJumpKeyDown())
        {
            if(!m_Grounded)
            {
                m_Rigidbody2d.velocity = new Vector2(m_Rigidbody2d.velocity.x, 0f); // Zero out the y velocity so the jump has equal effect, whether we're stationary, jumping or falling
            }

            m_Animator.SetBool("Ground", false);
            m_Rigidbody2d.AddForce(new Vector2(0, m_JumpForce));

            if (!m_DoubleJump && !m_Grounded)
            {
                m_DoubleJump = true;
            }
        }
    }

    // Update is called once per frame
    void FixedUpdate ()
    {
        m_Grounded = Physics2D.OverlapCircle(m_GroundCheck.position, m_GroundRadius, m_WhatIsGround);
        m_Animator.SetBool("Ground", m_Grounded);

        if (m_Grounded)
        {
            m_DoubleJump = false;
        }

        m_Animator.SetFloat("vSpeed", m_Rigidbody2d.velocity.y);

        float move = InputManager.Instance.GetLeftStickHorizontal();

        m_Animator.SetFloat("Speed", Mathf.Abs(move));
        m_Rigidbody2d.velocity = new Vector2(move * m_MaxSpeed, m_Rigidbody2d.velocity.y);

        if (move > 0 && !m_FacingRight)
        {
            flip();
        }
        else if (move < 0 && m_FacingRight)
        {
            flip();
        }
	}

    void flip()
    {
        m_FacingRight = !m_FacingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}
