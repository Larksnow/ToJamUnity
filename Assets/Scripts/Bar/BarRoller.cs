using UnityEngine;

public class BarRoller : MonoBehaviour
{
    public bool moveRight = true;            // Direction toggle
    public float moveSpeed = 1f;             // Units per second
    public float unitWidth = 1f;             // Width of each platform
    public int platformCount = 21;

    private float minX, maxX;

    void Start()
    {
        minX = 0f;
        maxX = unitWidth * (platformCount - 1);
    }

    void Update()
    {
        float step = moveSpeed * Time.deltaTime;
        foreach (Transform child in transform)
        {
            Vector3 pos = child.localPosition;

            // Move
            pos.x += (moveRight ? step : -step);

            // Wrap around
            if (pos.x > maxX)
                pos.x -= platformCount * unitWidth;
            else if (pos.x < minX - unitWidth)
                pos.x += platformCount * unitWidth;

            child.localPosition = pos;
        }
    }
}
