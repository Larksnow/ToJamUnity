using UnityEngine;
using DG.Tweening;

public class Killzone : MonoBehaviour
{
    public GameObject filling;
    public float activeTimer = 1f;

    public int targetID;
    public SpriteRenderer backgroundRenderer;
    public SpriteRenderer fillingRenderer;
    public Color[] backColors;
    public Color[] frontColors;

    private BoxCollider2D killzoneCollider;
    private bool isActive = false;

    void Start()
    {
        killzoneCollider = GetComponent<BoxCollider2D>();
        AudioManager.main.PostEvent("Play_ChargeAndShoot");

        if (killzoneCollider == null)
        {
            Debug.LogError("Killzone requires a BoxCollider2D.");
            return;
        }

        killzoneCollider.isTrigger = true;

        if (filling != null)
        {
            Vector3 startScale = filling.transform.localScale;
            filling.transform.localScale = new Vector3(0, startScale.y, startScale.z);

            filling.transform.DOScaleX(1f, activeTimer)
                .SetEase(Ease.InExpo)
                .OnComplete(() =>
                {
                    isActive = true;
                    Debug.Log("Killzone activated");

                    // Immediately play shake effect and destroy
                    transform.DOShakeScale(0.5f, 1.5f)
                             .OnComplete(() => Destroy(gameObject));
                });
        }
    }

    public void InitializeColor()
    {
        int callerID = (targetID == 0) ? 1 : 0;
        if (backgroundRenderer != null)
            backgroundRenderer.color = backColors[callerID];

        if (fillingRenderer != null)
            fillingRenderer.color = frontColors[callerID];
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        Player player = other.GetComponent<Player>();
        if (player != null && player.playerID == targetID)
        {
            Debug.Log("Player found and killed by trigger.");
            player.Die();
        }
    }
}
