using UnityEngine;
using System.Collections.Generic;

public class PlayerInteraction : MonoBehaviour
{
    public ItemSO[] items;
    public int maxHold = 3; // Max number of items player can carry
    public Player player;
    public ObjectEventSO itemChangeEvent;

    void Start()
    {
        player = GetComponent<Player>();
        items = new ItemSO[maxHold];
    }

    public void UseItem(int index)
    {
        if (index >= 0 && index < items.Length && items[index] != null)
        {
            AudioManager.main.PostEvent("Play_UseItem");
            items[index].UseItem(player);
            items[index] = null;
            itemChangeEvent.RaiseEvent(items, this);
        }
        else
        {
            Debug.LogWarning("Tried to use an invalid or null item at index: " + index);
        }
    }

    public void ItemPickUp(Item item)
    {
        ItemSO itemData = item.itemData;
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = itemData;
                Debug.Log($"Picked up item: {itemData.name} into slot {i}");
                Destroy(item.gameObject);
                itemChangeEvent.RaiseEvent(items, this);
                AudioManager.main.PostEvent("Play_PickUpItem");
                return;
            }
        }
        Debug.Log("Inventory full! Cannot pick up more items.");
    }

    public void ClearItems()
    {
        System.Array.Clear(items, 0, items.Length);
        itemChangeEvent.RaiseEvent(items, this);
    }
}
