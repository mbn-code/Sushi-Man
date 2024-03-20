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
    private bool Dead = false;

    [SerializeField]
    private GameObject LightObject;

    [SerializeField]
    private GameObject HeavyObject;

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
        if (Dead) return;

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
            if (!IsInAttackAnim())
            {
                StartCoroutine("EnableAndDisable", LightObject);
                AnimController.SetTrigger("Attack1");
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            if (!IsInAttackAnim())
            {
                StartCoroutine("EnableAndDisable", HeavyObject);
                AnimController.SetTrigger("Attack2");
            }
        }

        Flip();
    }

    private IEnumerator EnableAndDisable(GameObject Object)
    {
        Object.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Object.SetActive(false);
    }

    private void FixedUpdate()
    {
        if (Dead) return;

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

    public void KillMe()
    {
        AnimController.SetBool("Running", false);
        AnimController.SetBool("Falling", false);
        AnimController.SetTrigger("Death");
        Dead = true;
    }
}
