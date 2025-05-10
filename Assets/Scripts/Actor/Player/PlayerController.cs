using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void MovePlayer()
    {
        float horizontal = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontal, 0.0f).normalized;
        Vector2 position2D = new Vector2(transform.position.x, transform.position.y);
        rb.MovePosition(position2D + moveDirection * moveSpeed * Time.deltaTime);
    }
}
