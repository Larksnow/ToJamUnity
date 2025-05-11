using UnityEngine;
using DG.Tweening;

public class Killzone : MonoBehaviour
{
    public GameObject filling;
    public float activeTimer = 1f;
    public int targetID;

    private BoxCollider2D killzoneCollider;

    void Start()
    {
        killzoneCollider = GetComponent<BoxCollider2D>();
        AudioManager.main.PostEvent("Play_ChargeAndShoot");
        if (killzoneCollider == null)
        {
            Debug.LogError("Killzone requires a BoxCollider2D.");
            return;
        }

        if (filling != null)
        {
            Vector3 startScale = filling.transform.localScale;
            filling.transform.localScale = new Vector3(0, startScale.y, startScale.z);

            filling.transform.DOScaleX(1f, activeTimer)
                .SetEase(Ease.InExpo)
                .OnComplete(CheckAndKillPlayerThenDisappear);
        }
    }

    void CheckAndKillPlayerThenDisappear()
    {
        Debug.Log("2D Killzone activated — checking for player");

        Bounds bounds = killzoneCollider.bounds;
        Vector2 center = bounds.center;
        Vector2 size = bounds.size;

        Collider2D[] hits = Physics2D.OverlapBoxAll(center, size, 0f);
        foreach (Collider2D hit in hits)
        {
            Player player = hit.GetComponent<Player>();
            if (player != null && player.playerID == targetID)
            {
                Debug.Log("Player found and killed.");
                player.Die();
            }
        }

        // Optionally animate shrink and destroy after a short delay
        transform.DOShakeScale(0.5f, 1.5f)
             .OnComplete(() => Destroy(gameObject));
    }
}
