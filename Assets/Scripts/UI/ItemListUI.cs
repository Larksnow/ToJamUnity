using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;  // For InputSystem namespace

public class ItemListUI : MonoBehaviour
{
    public Image itemImage1;
    public Image itemImage2;
    public Image itemImage3;
    public TextMeshProUGUI itemText1;
    public TextMeshProUGUI itemText2;
    public TextMeshProUGUI itemText3;
    private Image[] itemImages;

    public void OnItemsChanged(object data)
    {
        if (data is ItemSO[] items)
        {
            Debug.Log("change itemUI");
            UpdateImage(items);
        }
        else
        {
            Debug.LogWarning("ItemListUI received non-ItemSO[] data.");
        }
    }

    public void UpdateImage(ItemSO[] items)
    {
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (i < items.Length && items[i] != null)
            {
                itemImages[i].sprite = items[i].icon;
                itemImages[i].enabled = true;
            }
            else
            {
                itemImages[i].enabled = false;
                itemImages[i].sprite = null;
            }
        }
    }

    void Start()
    {
        itemImages = new Image[] { itemImage1, itemImage2, itemImage3 };

        // Initial setup: disable all item slots
        foreach (var img in itemImages)
        {
            if (img != null)
                img.enabled = false;
        }
    }
}
