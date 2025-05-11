using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bar : MonoBehaviour
{
    private Rigidbody2D rb;
    public string rtpcName;
    private bool colorMode1 = true;
    public int index;
    private SpriteRenderer spriteRenderer;

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

    private float timer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        timer = stepInterval;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void FixedUpdate()
    {
        Vector2 currentPos = rb.position;

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
        if (colorMode1)
        {
            HeightToColor1(newY);
        }
        else
        {
            HeightToColor2();
        }        
        
        Vector2 targetPos = new Vector2(rb.position.x, newY);

        rb.MovePosition(targetPos);
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

    public void HeightToColor1(float height)
    {
        // Map height to a hue (e.g., from blue to red)
        float heightT = Mathf.InverseLerp(minHeight, maxHeight, height);
        float hue = Mathf.Lerp(0.65f, -0.2f, heightT); // 0.6 = blue, 0 = red
        Color newColor = Color.HSVToRGB(hue, 1f, 1f);
        spriteRenderer.color = newColor;
    }

    public void HeightToColor2()
    {
        // Map index 0–19 → hue 0–360 (wraps around red to red)
        float hue = (index / 19f); // hue in 0–1 for HSV
        float saturation = 1f;
        float value = 1f;

        Color color = Color.HSVToRGB(hue, saturation, value);

        // Apply color to the SpriteRenderer
        GetComponent<SpriteRenderer>().color = color;
    }
}
