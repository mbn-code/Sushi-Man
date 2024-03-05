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

        if(horizontal != 0 && IsGrounded())
        {
            if (!AnimController.GetBool("Running"))
                AnimController.SetBool("Running", true);
        } else
        {
            if (AnimController.GetBool("Running"))
                AnimController.SetBool("Running", false);
        }

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            if (AnimController.GetBool("Running"))
                AnimController.SetBool("Running", false);

            AnimController.SetTrigger("Jump");
            Rbody.velocity = new Vector2(Rbody.velocity.x, jumpingPower);
        }

        if(Input.GetButtonUp("Jump") && Rbody.velocity.y > 0f)
        {
            if (AnimController.GetBool("Running"))
                AnimController.SetBool("Running", false);

            AnimController.SetTrigger("Jump");
            Rbody.velocity = new Vector2(Rbody.velocity.x, Rbody.velocity.y * 0.5f);
        }

        if(Input.GetKeyDown(KeyCode.J))
        {
            if(!IsInAttackAnim())
                AnimController.SetTrigger("Attack1");
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!IsInAttackAnim())
                AnimController.SetTrigger("Attack2");
        }

        Flip();
    }

    private void FixedUpdate()
    {
        Rbody.velocity = new Vector2(horizontal * speed, Rbody.velocity.y);

        if (Rbody.velocity.y < 0f)
        {
            if (AnimController.GetBool("Running"))
                AnimController.SetBool("Running", false);

            if (!AnimController.GetBool("Falling"))
                AnimController.SetBool("Falling", true);
        }
        else
        {
            if (AnimController.GetBool("Falling"))
                AnimController.SetBool("Falling", false);
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private bool IsInAttackAnim()
    {
        if (AnimController.GetCurrentAnimatorStateInfo(0).IsName("Attack1") || AnimController.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
            return true;

        return false;
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
