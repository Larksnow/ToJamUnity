using UnityEngine;
using DG.Tweening;

public class FadeEffect : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public float fadeDuration = 0.2f;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void OnEnable()
    {
        // Fade in when the item is enabled
        if (spriteRenderer != null)
        {
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0); // Start from fully transparent
            spriteRenderer.DOFade(1f, fadeDuration); // Fade in to fully opaque
        }
    }

    
}
