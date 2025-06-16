using UnityEngine;
using System.Collections;

/// <summary>
/// Toggles the sprite on and off like a flash effect, for a certain number of times or duration.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class SpriteToggleFlasher : MonoBehaviour
{
    [Header("Flash Settings")]
    public float flashInterval = 0.2f;     // Time between on/off
    public int flashCount = 6;             // Total number of flashes (on + off = 1 flash)

    private SpriteRenderer spriteRenderer;
    private Coroutine flashRoutine;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false; // Start hidden
    }

    void Start()
    {
        spriteRenderer.enabled = false; // Start hidden
    }

    /// <summary>
    /// Starts the on/off flashing.
    /// </summary>
    public void StartFlashing()
    {
        if (flashRoutine != null)
            StopCoroutine(flashRoutine);

        flashRoutine = StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        for (int i = 0; i < flashCount; i++)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
            yield return new WaitForSeconds(flashInterval);
        }

        spriteRenderer.enabled = false; // Make sure it's hidden at the end
        flashRoutine = null;
    }

    /// <summary>
    /// Instantly stops flashing and hides the sprite.
    /// </summary>
    public void StopFlashing()
    {
        if (flashRoutine != null)
        {
            StopCoroutine(flashRoutine);
            flashRoutine = null;
        }
        spriteRenderer.enabled = false;
    }
}
