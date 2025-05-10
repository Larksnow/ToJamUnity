using System;
using UnityEngine;

public class PlatformLauncher : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private BoxCollider2D collider;
    [SerializeField] private float launchForce = 2000.0f;
    [SerializeField] private float launchDuration = 4.0f;
    [SerializeField] private float pushForce = 100.0f;
    [SerializeField] private float checkDistance = 0.2f;

    private float currentTime = 0.0f;
    private Vector2 previousVelocity = Vector2.zero;

    bool IsNearlyZero(float val)
    {
        return Mathf.Abs(val) < 0.01f;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        // check if the cube stops ascending. If so, apply forces on the contacted objects.
        if (!IsNearlyZero(rb.linearVelocity.y) && !IsNearlyZero(previousVelocity.y) && 
            Mathf.Abs(rb.linearVelocity.y) < 0.1f && previousVelocity.y > 0.0f)
        {
            ApplyForce();
        }
        
        if (currentTime >= launchDuration)
        {
            currentTime = 0.0f;
            LaunchActor();
        }
        
        previousVelocity = rb.linearVelocity;
    }

    void LaunchActor()
    {
        rb.AddForce(Vector2.up * launchForce, ForceMode2D.Impulse);
    }

    void ApplyForce()
    {
        Vector3 offset = new Vector3(0, checkDistance, 0);
        Vector3 extents = new Vector3(0.9f, 0.9f, 0.9f);
        RaycastHit2D[] hitColliders = Physics2D.BoxCastAll(collider.bounds.center + offset, extents, 0.0f, Vector2.up);
        
        foreach (var raycast in hitColliders)
        {
            Collider2D collider = raycast.collider;
            if (collider != null && collider.gameObject.CompareTag("Bouncer"))
            {
                Rigidbody2D otherRb = collider.gameObject.GetComponent<Rigidbody2D>();
                if (otherRb != null)
                {
                    otherRb.linearVelocity = Vector2.zero;
                    otherRb.AddForce(Vector2.up * pushForce, ForceMode2D.Impulse);
                    Debug.Log("Pushing object: " + collider.gameObject.name);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Vector3 offset = new Vector3(0, checkDistance, 0);
        Vector3 extents = new Vector3(0.9f, 0.9f, 0.9f);
        Gizmos.DrawCube(collider.bounds.center + offset, extents);
        Gizmos.DrawLine(collider.transform.position, new Vector3(collider.transform.position.x, collider.transform.position.y + checkDistance));
    }
}
