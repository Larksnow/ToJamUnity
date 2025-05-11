using UnityEngine;
using System.Collections; // Required for IEnumerator
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float jumpDelay = 0.2f;
    private Vector2 moveInput = Vector2.zero;

    public float horizontalDampening = 0.5f;

    public GameObject landCloudPrefab;
    
    [Header("State")]
    [SerializeField] private bool _isGrounded;
    public bool IsGrounded { get => _isGrounded; private set => _isGrounded = value; }
    
    private Rigidbody2D rb;
    private Animator animator;
    private Vector3 originalScale;
    private bool isFacingRight = true;
    public SpriteRenderer outfitRenderer;
    public Color[] playerColors;
    public int playerID;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        originalScale = transform.localScale;

        var playerInput = GetComponent<PlayerInput>();
        playerID = playerInput.playerIndex;

        if (outfitRenderer != null && playerID < playerColors.Length)
        {
            outfitRenderer.color = playerColors[playerID];
        }
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
    
    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    
    void HandleMovement()
    {
        // Apply horizontal movement
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);

        animator.SetFloat("moveSpeed", Mathf.Abs(moveInput.x));

        // Flip character based on movement direction
        if (moveInput.x > 0 && !isFacingRight || moveInput.x < 0 && isFacingRight)
            Flip();

        WrapAroundScreen();
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

    void WrapAroundScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        // Horizontal wrap
        if (viewPos.x > 1f)
            viewPos.x = 0f;
        else if (viewPos.x < 0f)
            viewPos.x = 1f;

        // Safety check: if player falls below -6 world Y, reset position
        if (transform.position.y < -6f)
        {
            transform.position = Vector3.zero;
            rb.linearVelocity = Vector2.zero;
            return;
        }

        transform.position = Camera.main.ViewportToWorldPoint(viewPos);
    }

    public void Die()
    {
        Debug.Log("Player " + playerID + " died");
        AudioManager.main.PostEvent("Play_Death");
    }

}
