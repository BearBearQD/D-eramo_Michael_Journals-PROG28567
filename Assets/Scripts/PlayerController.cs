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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        MovementUpdate(playerInput);

        anim.SetBool("IsWalking", IsWalking());
    }

    private void MovementUpdate(Vector2 playerInput)
    {
        rb.linearVelocity = new Vector2(playerInput.x * moveSpeed, rb.linearVelocity.y);

        if (playerInput.x > 0)
            facing = FacingDirection.right;
        else if (playerInput.x < 0)
            facing = FacingDirection.left;
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
        }

    }

    private void OnCollisionExit(Collision collision)
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
}
