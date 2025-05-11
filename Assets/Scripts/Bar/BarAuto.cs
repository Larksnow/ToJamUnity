using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class BarAuto : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D boxCollider;
    
    [Header("Setting")]
    [SerializeField] private float flipDuration = 2.0f;
    [SerializeField] private float pushHeight = 2.0f;  
    [SerializeField] private float checkDistance = 5.0f;

    [Header("UpwardForce")]
    [SerializeField] private float maxHeight = 10.0f; // depends on frequency
    [SerializeField] private float exponent = 2.5f; // for acceleration curve
    [SerializeField] private float maxForce = 4000.0f;  // this can move to player itself
    [SerializeField] private float localAccelerationFactor = 20.0f;

    // for auto movement
    private Vector3 originalPosition;
    private bool moveUpward = true;
    private float timer;
    
    private List<Rigidbody2D> colliders  = new List<Rigidbody2D>();
    private bool waitForTick = false;

    void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        originalPosition =  transform.position;
        timer = flipDuration;
    }

    void FixedUpdate()
    {
        if (waitForTick)
        {
            RemoveContactedInstance();
            waitForTick = false;
        }
        
        timer -= Time.fixedDeltaTime;
        if (timer <= 0f)
        {
            if (moveUpward)
            {
                CollectContactedInstance();
            }
            
            // --- Vertical movement ---
            rb.MovePosition(CalculatePosition());
            
            timer = flipDuration;
            moveUpward = !moveUpward;
        }
    }

    public Vector2 CalculatePosition()
    {
        if (moveUpward)
        {
            return new Vector2(originalPosition.x, originalPosition.y + pushHeight);
        }
        
        return originalPosition;
    }

    void CollectContactedInstance()
    {
        Vector3 center = transform.position;
        Vector3 size = boxCollider.size;
        Vector3 upperEdgePosition = new Vector3(center.x, center.y + size.y / 2, center.z);
        Vector3 offset = new Vector3(0.1f, 0.1f, 0.0f);
        Vector3 extents = new Vector3(size.x / 2, size.y / 2, 0.0f);
        RaycastHit2D[] hitColliders = Physics2D.BoxCastAll(upperEdgePosition, extents - offset, 0.0f, Vector2.up);
        
        foreach (var raycast in hitColliders)
        {
            Collider2D collider = raycast.collider;
            if (collider != null)
            {
                Rigidbody2D otherRb = collider.gameObject.GetComponent<Rigidbody2D>();
                if (otherRb != null && otherRb != rb)
                {
                    colliders.Add(otherRb);
                }
            }
        }

        if (colliders.Count > 0)
        {
            waitForTick = true;
        }
    }

    void RemoveContactedInstance()
    {
        foreach (var instance in colliders)
        {
            if (instance == null)
            {
                continue;
            }
            
            // remove huge force on the contacted instances and apply another upward force on them.
            instance.linearVelocityY = 0.0f;
            float normalizedHeight = Mathf.Clamp01(pushHeight / maxHeight);
            float mappedHeight = Mathf.Pow(normalizedHeight, exponent) * maxHeight;
            float force = Mathf.Clamp(mappedHeight * instance.mass * localAccelerationFactor, 0, maxForce);
            instance.AddForce(new Vector2(0, force), ForceMode2D.Impulse);
            Debug.Log("Pushing object: " + instance.gameObject.name + " with force: " +  force);
        }
        
        colliders.Clear();
    }
}
