using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public ItemSO itemSo;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object has a PlayerInteraction component
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction != null && playerInteraction.items.Count < playerInteraction.maxHold)
        {
            playerInteraction.ItemPickUp(itemSo);
            Destroy(gameObject); // Remove item from the scene
        }
    }

    void Start() { }

    void Update() { }
}
