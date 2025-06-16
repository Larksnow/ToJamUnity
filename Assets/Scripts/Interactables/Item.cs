using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemData;

    public bool isPickedUp = false;  // Prevent multiple pickups

    void OnTriggerEnter2D(Collider2D other)
    {
        if (isPickedUp) return;  // Already picked up, ignore

        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        {
            playerInteraction.ItemPickUp(this);
        }
    }
}
