using System.Collections;
using UnityEngine;

public class DeleteAfterSeconds : MonoBehaviour
{
    [SerializeField] float seconds = 15f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float flashInterval = 0.2f;

    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        StartCoroutine(Death());
    }

    IEnumerator Death()
    {
        float flashStart = Mathf.Max(0, seconds - 3f);

        // Wait until flashing should begin
        yield return new WaitForSeconds(flashStart);

        // Start flashing for the last 3 seconds
        float elapsed = 0f;
        while (elapsed < 3f)
        {
            FlashSprite();
            yield return new WaitForSeconds(flashInterval);
            elapsed += flashInterval;
        }
        
        Collider2D col = GetComponent<Collider2D>();
        if (col != null)
        {
            col.enabled = false;
        }
        Destroy(gameObject);
    }

    void FlashSprite()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = !spriteRenderer.enabled;
        }
    }
}
