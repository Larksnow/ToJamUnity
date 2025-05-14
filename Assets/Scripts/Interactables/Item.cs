using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemSO itemData;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object has a PlayerInteraction component
        PlayerInteraction playerInteraction = other.GetComponent<PlayerInteraction>();
        if (playerInteraction != null) playerInteraction.ItemPickUp(this);
    }

    void Start() { }

    void Update() { }
}
