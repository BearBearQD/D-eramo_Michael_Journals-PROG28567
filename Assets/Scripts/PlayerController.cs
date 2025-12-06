using System.Collections;
using System.Timers;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum FacingDirection
    {
        left, right
    }

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private FacingDirection facing = FacingDirection.right;
    private Animator anim;

    public bool grounded = false;

    public float apexheight = 3f;
    public float apexTime = 0.5f;
    public float terminalSpeed = 10f;
    public float coyoteTime = 0.2f;

    private float coyotetimer = 0f;
    private float jumpGravity;
    private float initialJumpVelocity;
    private float VerticaljumpVelocity;
    private bool isjump = false;

    public float dashForce = 20f;
    public float dashCooldown = 1f;
    public float dashDuration = 0.15f;
    private float nextDashTime = 0f;
    private bool isDashing;

    public int maxJumps = 3;
    public int jumpsRemaining;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        jumpGravity = -2f * apexheight/(apexTime*apexTime);
        initialJumpVelocity = 2f * apexheight / apexTime;
        jumpsRemaining = maxJumps;

        rb.gravityScale = 0f;
    }

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"),0);
        MovementUpdate(playerInput);



        anim.SetBool("IsWalking", IsWalking());

        if(Input.GetKeyDown(KeyCode.E) && Time.time >= nextDashTime)
        {
            Dash();
        }
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        if (!grounded)
        {
            coyotetimer -= Time.deltaTime;
        }
        else
        {
            coyotetimer = coyoteTime;
        }

        Jumpmotion();

        rb.linearVelocity = new Vector2(playerInput.x * moveSpeed, VerticaljumpVelocity);

        if (playerInput.x > 0)
            facing = FacingDirection.right;
        else if (playerInput.x < 0)
            facing = FacingDirection.left;

        VerticaljumpVelocity = Mathf.Max(VerticaljumpVelocity, -terminalSpeed);

    }

    public bool IsWalking()
    {
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
        {
            return true;
        }
        else { return false; }
            
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            grounded = true;
            isjump = false;
            VerticaljumpVelocity = 0f;
            jumpsRemaining = maxJumps;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if(collision.collider.CompareTag("Ground"))
        {
            grounded = false;
        }
    }


    public bool IsGrounded()
    {
        return grounded;
    }

    public FacingDirection GetFacingDirection()
    {
        return facing;
    }

    private void Jumpmotion()
    {
        if (Input.GetKeyDown(KeyCode.Space) && jumpsRemaining == maxJumps)
        {
            if (grounded && coyotetimer > 0f)
            {

                DoJump();
                return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) && !grounded && jumpsRemaining > 0)
        {
            DoJump();
        }

        if (!grounded)
        {
            VerticaljumpVelocity += jumpGravity * Time.deltaTime;;
        }

        else
        {
            if (!isjump)
            {
                VerticaljumpVelocity = 0f;
            }
        }
    }

    private void Dash()
    {
        if (isDashing)
        {
            return;
        }

        nextDashTime = Time.time + dashCooldown;
        StartCoroutine(DashRoutine());
    }

    private IEnumerator DashRoutine()
    {
        isDashing = true;

        float dashDirection = (facing == FacingDirection.right) ? 1f : -1f;
        float elapsed = 0f;

        float storedGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        while (elapsed < dashDuration)
        {
            float t = elapsed / dashDuration;
            float smoothSpeed = Mathf.Lerp(dashForce, 0, t);

            rb.linearVelocity = new Vector2(smoothSpeed * dashDirection, 0f);

            elapsed += Time.deltaTime;
            yield return null;
        }

        rb.gravityScale = storedGravity;

        isDashing = false;
    }

    private void DoJump()
    {
        isjump = true;
        grounded = false;
        VerticaljumpVelocity = initialJumpVelocity;

        jumpsRemaining--;
    }
}
