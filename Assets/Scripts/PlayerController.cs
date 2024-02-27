using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    [SerializeField]
    private Rigidbody2D Rbody;

    [SerializeField]
    private Transform groundCheck;

    [SerializeField]
    private LayerMask groundLayer;
    
    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            Rbody.velocity = new Vector2(Rbody.velocity.x, jumpingPower);
        }

        if(Input.GetButtonUp("Jump") && Rbody.velocity.y > 0f)
        {
            Rbody.velocity = new Vector2(Rbody.velocity.x, Rbody.velocity.y * 0.5f);
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
