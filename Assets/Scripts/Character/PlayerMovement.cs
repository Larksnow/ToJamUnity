using UnityEngine;
using System.Collections;
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
    private GameObject currentLandCloud;

    [Header("Ground Check via Raycast")]
    [SerializeField] private Transform groundCheckPoint;
    [SerializeField] private float groundCheckDistance = 0.2f;
    [SerializeField] private LayerMask groundLayer;

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
    private PlayerInteraction interaction;
    private bool wasGroundedLastFrame = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponentInChildren<Animator>();
        originalScale = transform.localScale;
        interaction = GetComponent<PlayerInteraction>();

        if (outfitRenderer != null && playerID < playerColors.Length)
        {
            outfitRenderer.color = playerColors[playerID];
        }
    }

    void LateUpdate()
    {
        transform.rotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void Update()
    {
        RaycastGroundCheck();
        HandleJump();
        UpdateAnimationState();
    }

    public void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    public void OnUseItem1() => interaction.UseItem(0);
    public void OnUseItem2() => interaction.UseItem(1);
    public void OnUseItem3() => interaction.UseItem(2);

    void HandleMovement()
    {
        rb.linearVelocity = new Vector2(moveInput.x * moveSpeed, rb.linearVelocity.y);
        animator.SetFloat("moveSpeed", Mathf.Abs(moveInput.x));

        if (moveInput.x > 0 && !isFacingRight || moveInput.x < 0 && isFacingRight)
            Flip();

        WrapAroundScreen();
    }

    void HandleJump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            IsGrounded = false;
            animator.SetTrigger("Jump");
            StartCoroutine(ApplyJumpForceAfterDelay(jumpDelay));
        }
    }

    void UpdateAnimationState()
    {
        if (!IsGrounded)
        {
            animator.SetBool("IsFalling", rb.linearVelocity.y < -0.1f);
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

    void RaycastGroundCheck()
    {
        Vector2 origin = groundCheckPoint != null ? groundCheckPoint.position : transform.position;
        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.down, groundCheckDistance, groundLayer);

        IsGrounded = hit.collider != null;

        Debug.DrawRay(origin, Vector2.down * groundCheckDistance, IsGrounded ? Color.green : Color.red);

        if (!wasGroundedLastFrame && IsGrounded)
        {
            animator.SetTrigger("Land");
            if (currentLandCloud != null) Destroy(currentLandCloud);
            currentLandCloud = Instantiate(landCloudPrefab, transform.position, Quaternion.identity);
        }

        wasGroundedLastFrame = IsGrounded;
    }

    private IEnumerator ApplyJumpForceAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        animator.ResetTrigger("Jump");
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce * 3);
    }

    void WrapAroundScreen()
    {
        Vector3 viewPos = Camera.main.WorldToViewportPoint(transform.position);

        if (viewPos.x > 1f) viewPos.x = 0f;
        else if (viewPos.x < 0f) viewPos.x = 1f;

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
        Scoreboard.Instance.AddScore(playerID);
        AudioManager.main.PostEvent("Play_Death");
    }
}
