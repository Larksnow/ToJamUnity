using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveForce = 50f;         // How strong the input force is
    public float maxSpeed = 5f;           // Cap horizontal speed
    public float linearDrag = 4f;         // Drag to smooth out movement

    private Rigidbody2D rb;
    private float inputX;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearDamping = linearDrag;  // Helps stop sliding
    }

    void Update()
    {
        inputX = Input.GetAxisRaw("Horizontal"); // -1 (left), 0, 1 (right)
    }

    void FixedUpdate()
    {
        // Add force only if under max speed
        if (Mathf.Abs(rb.linearVelocity.x) < maxSpeed || Mathf.Sign(inputX) != Mathf.Sign(rb.linearVelocity.x))
        {
            rb.AddForce(Vector2.right * inputX * moveForce);
        }
    }
}
