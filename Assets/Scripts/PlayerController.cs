using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;
    private Animator AnimController;

    [SerializeField]
    private Rigidbody2D Rbody;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;

    private void Awake()
    {
        AnimController = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Rbody.velocity = new Vector2(Rbody.velocity.x, jumpingPower);
            AnimController.SetTrigger("Jump");
        }

        if(Input.GetButtonUp("Jump") && Rbody.velocity.y > 0f)
        {
            Rbody.velocity = new Vector2(Rbody.velocity.x, Rbody.velocity.y * 0.5f);
            AnimController.SetTrigger("Jump");
        }

        if (Rbody.velocity.y < 0f)
        {
            if(!AnimController.GetBool("Falling"))
                AnimController.SetBool("Falling", true);
        } else
        {
            if (AnimController.GetBool("Falling"))
                AnimController.SetBool("Falling", false);
        }

        if(Rbody.velocity.x > 0.05f)
        {
            if (!AnimController.GetBool("Running"))
                AnimController.SetBool("Running", true);
        } else
        {
            if (AnimController.GetBool("Running"))
                AnimController.SetBool("Running", false);
        }

        Flip();
    }

    private void FixedUpdate()
    {
        Rbody.velocity = new Vector2(horizontal * speed, Rbody.velocity.y);
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if(isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
