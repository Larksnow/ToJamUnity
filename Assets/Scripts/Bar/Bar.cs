using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bar : MonoBehaviour
{
    private Rigidbody2D rb;
    public string rtpcName;
    public int index;

    [Header("Vertical Mapping")]
    private float minDb = -18f;
    private float maxDb = 0f;
    private float minHeight = -11f;
    private float maxHeight = -6f;

    [Header("Horizontal Step Move")]
    public bool moveRight = true;
    private float stepInterval = 30f/136f; // time between steps
    private float stepSize = 0.5f;     // distance per step
    private float minX = -10.5f;
    private float maxX = 10.5f;
    
    [Header("UpwardForce")]
    private float exponent = 2.5f; // for acceleration curve
    private float maxForce = 1000.0f;  // this can move to player itself
    private float localAccelerationFactor = 50.0f;
    private float checkDistance = 1.0f;

    private float timer;
    private float heightDelta = -1.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        timer = stepInterval;
    }

    void FixedUpdate()
    {
        Vector2 currentPos = rb.position;

        if (heightDelta > 0)
        {
            LaunchContactedInstances(heightDelta, true);
        }

        // --- Horizontal step: teleport X ---
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            timer = stepInterval;

            float newX = currentPos.x + (moveRight ? stepSize : -stepSize);

            // Wrap around if needed
            if (newX > maxX) newX = minX;
            else if (newX < minX) newX = maxX;

            // Teleport X (instant change)
            transform.position = new Vector2(newX, transform.position.y);
        }

        // --- Vertical movement (RTPC + smooth) ---
        float newY = AudioManager.main.GetRTPCValue(rtpcName);
        newY = DbToHeight(newY);
        // newY -= IndexToOffset(index);
        // newY = Mathf.Clamp(newY, minHeight, maxHeight);
        Vector2 targetPos = new Vector2(rb.position.x, newY);

        rb.MovePosition(targetPos);
        heightDelta = newY - currentPos.y;
        
        if (heightDelta > 0)
        {
            LaunchContactedInstances(heightDelta, false);
        }
    }

    public float DbToHeight(float input)
    {
        input = Mathf.Clamp(input, minDb, maxDb);
        float t = (input - minDb) / (maxDb - minDb);
        return Mathf.Lerp(minHeight, maxHeight, t);
    }

    public float IndexToOffset(int index)
    {
        // Clamp index to 0–9
        index = Mathf.Clamp(index, 0, 9);

        // Map index to a t-value in [0, 1]
        float t = index / 9f;

        return Mathf.Lerp(0f, 3f, t);
    }
    
    void LaunchContactedInstances(float heightDelta, bool applyForce)
    {
        Vector3 center = transform.position;
        Vector3 size = transform.localScale;
        Vector3 upperEdgePosition = new Vector3(center.x, center.y + size.y / 2, center.z);
        Vector3 extents = new Vector3(size.x / 2, checkDistance);
        RaycastHit2D[] hitColliders = Physics2D.BoxCastAll(upperEdgePosition, extents, 0.0f, Vector2.up, 0.1f);
        Debug.DrawLine(center, center + new Vector3(0, size.y / 2 + checkDistance), Color.red, 2.0f);
        
        float mappedHeight = Mathf.Pow(heightDelta, exponent) * Mathf.Abs(maxHeight);
        
        foreach (var raycast in hitColliders)
        {
            Collider2D collider = raycast.collider;
            if (collider != null && collider.gameObject.CompareTag("Bouncer"))
            {
                Rigidbody2D otherRb = collider.gameObject.GetComponent<Rigidbody2D>();
                if (otherRb != null)
                {
                    otherRb.linearVelocityY = 0.0f;
                    
                    float force = Mathf.Clamp(mappedHeight * otherRb.mass * localAccelerationFactor, 0, maxForce);
                    otherRb.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
                    Debug.Log(gameObject.name + " pushing object: " + otherRb.gameObject.name + " with force: " + force);
                }
            }
        }
    }
}
