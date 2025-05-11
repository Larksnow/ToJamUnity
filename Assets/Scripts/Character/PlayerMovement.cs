using UnityEngine;
using System.Collections; // Required for IEnumerator

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float maxJumpSpeed = 25.0f;
    public float jumpForce = 5f;
    public float jumpDelay = 0.2f;

    public float horizontalDampening = 0.5f;

    public GameObject landCloudPrefab;
    
    [Header("State")]
    [SerializeField] private bool _isGrounded;
    public bool IsGrounded { get => _isGrounded; private set => _isGrounded = value; }
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalScale;
    private bool isFacingRight = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;
    }
    void FixedUpdate()
    {
        HandleMovement();
    }
    
    void Update()
    {
        HandleJump();
        UpdateAnimationState();
    }
    
    void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, Mathf.Min(rb.linearVelocity.y, maxJumpSpeed));
        
        animator.SetFloat("moveSpeed", Mathf.Abs(moveInput));
        
        // Flip character based on movement direction
        if (moveInput > 0 && !isFacingRight || moveInput < 0 && isFacingRight)
            Flip();
    }
    
    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            IsGrounded = false; // prevent double jumps
            animator.SetTrigger("Jump");
            StartCoroutine(ApplyJumpForceAfterDelay(jumpDelay));
        }
    }

    void UpdateAnimationState()
    {
        // Jumping/Falling animation states
        if (!IsGrounded)
        {
            if (rb.linearVelocity.y > 0.1f)
            {
                animator.SetBool("IsFalling", false);
            }
            else if (rb.linearVelocity.y < -0.1f)
            {
                animator.SetBool("IsFalling", true);
            }
        }
        else
        {
            animator.SetBool("IsFalling", false);
        }
    }
    
    void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.contacts[0].normal.y > 0.5f)
        {
            IsGrounded = true;
            animator.SetTrigger("Land");

            Instantiate(landCloudPrefab, transform.position, Quaternion.identity);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        IsGrounded = false;
    }

    private IEnumerator ApplyJumpForceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce*3);
    }
}
