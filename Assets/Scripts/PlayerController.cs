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


    private float jumpGravity;
    private float initialJumpVelocity;
    private float VerticaljumpVelocity;
    private bool isjump = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        jumpGravity = -2f * apexheight/(apexTime*apexTime);
        initialJumpVelocity = 2f * apexheight / apexTime;

        rb.gravityScale = 0f;
    }

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Jump"));
        MovementUpdate(playerInput);

        anim.SetBool("IsWalking", IsWalking());
        print(VerticaljumpVelocity);
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        Jumpmotion(playerInput);
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

    private void Jumpmotion(Vector2 movementInput)
    {
        if (movementInput.y > 0f && grounded)
        { isjump = true;
            grounded = false;
            VerticaljumpVelocity = initialJumpVelocity;
        }
        
        if (!grounded)
        {
            VerticaljumpVelocity += jumpGravity * Time.deltaTime;
        }

        else
        {
            if (!isjump)
            {
                VerticaljumpVelocity = 0f;
            }
        }
    }
}
