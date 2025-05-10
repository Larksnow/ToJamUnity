using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bar : MonoBehaviour
{
    private Rigidbody2D rb;
    public string rtpcName;

    [Header("Vertical Mapping")]
    public float minDb = -12f;
    public float maxDb = 0f;
    public float minHeight = -11f;
    public float maxHeight = -6f;

    [Header("Horizontal Step Move")]
    public bool moveRight = true;
    private float stepInterval = 30f/136f; // time between steps
    public float stepSize = 1f;     // distance per step
    private float minX = -10f;
    private float maxX = 10f;

    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        timer = stepInterval;
    }

    void FixedUpdate()
    {
        // --- Vertical RTPC ---
        float newY = AudioManager.main.GetRTPCValue(rtpcName);
        newY = DbToHeight(newY);

        // --- Horizontal step logic ---
        Vector2 currentPos = rb.position;
        timer -= Time.fixedDeltaTime;

        if (timer <= 0f)
        {
            timer = stepInterval;

            float newX = currentPos.x + (moveRight ? stepSize : -stepSize);

            // Wrap around
            if (newX > maxX)
                newX = minX;
            else if (newX < minX)
                newX = maxX;

            currentPos = new Vector2(newX, newY);
        }
        else
        {
            // Only update Y if not stepping X
            currentPos = new Vector2(currentPos.x, newY);
        }

        rb.MovePosition(currentPos);
    }

    public float DbToHeight(float input)
    {
        input = Mathf.Clamp(input, minDb, maxDb);
        float t = (input - minDb) / (maxDb - minDb);
        return Mathf.Lerp(minHeight, maxHeight, t);
    }
}
