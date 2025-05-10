using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bar : MonoBehaviour
{
    private Rigidbody2D rb;
    public string rtpcName;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
    }

    void FixedUpdate()
    {
        float newY = AudioManager.main.GetRTPCValue(rtpcName);
        newY = DbToHeight(newY);
        Vector2 currentPos = rb.position;
        Vector2 targetPos = new Vector2(currentPos.x, newY);

        rb.MovePosition(targetPos);
        
    }

    public float DbToHeight(float input)
    {
        // Clamp input to ensure it's within expected bounds
        input = Mathf.Clamp(input, -12f, 0f);

        // Normalize input from [-12, 0] to [0, 1]
        float t = (input + 12f) / 12f;

        // Map to target range [-11, -6]
        float output = Mathf.Lerp(-11f, -6f, t);

        return output;
    }

}
