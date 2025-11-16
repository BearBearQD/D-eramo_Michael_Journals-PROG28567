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

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // The input from the player needs to be determined and
        // then passed in the to the MovementUpdate which should
        // manage the actual movement of the character.
        Vector2 playerInput = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        MovementUpdate(playerInput);
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
        return Mathf.Abs(rb.angularVelocity) > 0.1f ;
    }
    public bool IsGrounded()
    {
        return false;
    }

    public FacingDirection GetFacingDirection()
    {
        return facing;
    }
}
