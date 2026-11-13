using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 7f;
    [SerializeField] private float jumpForce = 12f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private Rigidbody2D rb;
    private float moveInput;
    private bool jumpRequested;
    private bool facingRight = true;

    public bool FacingRight => facingRight;

    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        moveInput = Mathf.Clamp(Input.GetAxisRaw("Horizontal") + MobileControlButton.Horizontal, -1f, 1f);
        if (Input.GetKeyDown(KeyCode.Space) || MobileControlButton.ConsumeJump()) jumpRequested = true;

        if (moveInput > 0.01f && !facingRight) Flip();
        else if (moveInput < -0.01f && facingRight) Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(moveInput * moveSpeed, rb.velocity.y);
        if (jumpRequested)
        {
            if (IsGrounded()) rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    private bool IsGrounded()
    {
        if (groundCheck == null) return Mathf.Abs(rb.velocity.y) < 0.05f;
        return Physics2D.OverlapCircle(groundCheck.position, 0.18f, groundLayer);
    }

    private void Flip()
    {
        facingRight = !facingRight;
        Vector3 s = transform.localScale;
        s.x *= -1;
        transform.localScale = s;
    }
}
