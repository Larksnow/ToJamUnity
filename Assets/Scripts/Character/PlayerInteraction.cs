using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public List<ItemSO> items = new List<ItemSO>();
    public int maxHold = 3; // Max number of items player can carry
    public Player player;

    void Start()
    {
        player = GetComponent<Player>();
    }

    void Update()
    {
        // Example: use first item
        // if (Input.GetKeyDown(KeyCode.E)) UseItem(0);
    }

    public void UseItem(int index)
    {
        if (index >= 0 && index < items.Count && items[index] != null)
        {
            items[index].UseItem(player);
            items.RemoveAt(index);
        }
        else
        {
            Debug.LogWarning("Tried to use an invalid or null item at index: " + index);
        }
    }

    public void ItemPickUp(ItemSO itemSo)
    {
        if (items.Count >= maxHold)
        {
            Debug.Log("Inventory full! Cannot pick up more items.");
            return;
        }

        items.Add(itemSo);
        Debug.Log($"Picked up item: {itemSo.name}");
    }
}
