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
        killzoneCollider.enabled = false;

        if (filling != null)
        {
            Vector3 startScale = filling.transform.localScale;
            filling.transform.localScale = new Vector3(0, startScale.y, startScale.z);

            filling.transform.DOScaleX(1f, activeTimer)
                .SetEase(Ease.InExpo)
                .OnComplete(() =>
                {
                    isActive = true;
                    ActivateKillzone();
                    Debug.Log("Killzone activated");
                    // Immediately play shake effect and destroy
                    transform.DOShakeScale(0.2f, 1.5f)
                        .OnComplete(() => {
                            killzoneCollider.enabled = false;
                            Destroy(gameObject);
                        });
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

    void ActivateKillzone()
    {
        isActive = true;
        killzoneCollider.enabled = true; // Enable collider at the activation moment

        // Now only players that ENTER after this moment will trigger OnTriggerEnter2D
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActive) return;

        Player player = other.GetComponent<Player>();
        if (player != null && player.playerID == targetID)
        {
            Debug.Log("Player entered and was killed.");
            CameraShake.main.TriggerShake();
            killzoneCollider.enabled = false;
            player.Die();
        }
    }
}
