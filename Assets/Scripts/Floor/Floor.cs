using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Floor : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        float randomY = Random.Range(-3.5f, -2.5f);
        Vector2 currentPos = rb.position;
        Vector2 targetPos = new Vector2(currentPos.x, randomY);

        rb.MovePosition(targetPos);
    }
}
