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

    [SerializeField] internal bool DashingUnlocked = false;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 5f;

    [SerializeField] private GameObject LightObject;
    [SerializeField] private GameObject HeavyObject;
    [SerializeField] private Rigidbody2D Rbody;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private Spawner SpawnerInstance;
    [SerializeField] internal GameManager GM;



    private void Awake()
    {
        AnimController = GetComponent<Animator>();
    }

    void Update()
    {
        if (Dead || isDashing) return;

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

        if (DashingUnlocked && canDash && Input.GetKeyDown(KeyCode.LeftShift))
        {
            StartCoroutine("Dash");
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
        yield return new WaitForSeconds(0.35f);
        Object.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        Object.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(Dead || isDashing) return;

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

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = Rbody.gravityScale;
        Rbody.gravityScale = 0f;
        Rbody.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        yield return new WaitForSeconds(dashingTime);
        Rbody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    public void KillMe()
    {
        AnimController.SetBool("Running", false);
        AnimController.SetBool("Falling", false);
        AnimController.SetTrigger("Death");
        Dead = true;
        SpawnerInstance.DeleteAllBalls();
    }

    internal void RemoveBallFromSpawner(GameObject BallObject)
    {
        SpawnerInstance.RemoveBall(BallObject);
    }
}
