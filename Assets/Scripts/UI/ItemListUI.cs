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
            UpdateImage(items);
        }
        else
        {
            Debug.LogWarning("ItemListUI received non-ItemSO[] data.");
        }
    }

    public void OnSchemeChanged(object data)
    {
        if (data is string deviceKey)
        {
            UpdateText(deviceKey);
        }
    }
    public void UpdateText(string deviceKey)
    {
        string actionKey1 = "UseItem1";
        string actionKey2 = "UseItem2";
        string actionKey3 = "UseItem3";
        itemText1.text = GetDisplayBinding(actionKey1, deviceKey);
        itemText2.text = GetDisplayBinding(actionKey2, deviceKey);
        itemText3.text = GetDisplayBinding(actionKey3, deviceKey);
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

    public string GetDisplayBinding(string actionName, string deviceKey)
    {
        if (hardcodedBindings.TryGetValue(actionName, out var deviceMap))
        {
            if (deviceMap.TryGetValue(deviceKey, out var binding))
            {
                return binding;
            }
        }
        return "N/A";
    }

    Dictionary<string, Dictionary<string, string>> hardcodedBindings = new Dictionary<string, Dictionary<string, string>>()
    {
        { "UseItem1", new Dictionary<string, string> {
            { "Keyboard_P1", "Q" },
            { "Keyboard_P2", "J" },
            { "Xbox", "X" },
            { "PlayStation", "■" } // Square button
        }},
        { "UseItem2", new Dictionary<string, string> {
            { "Keyboard_P1", "W" },
            { "Keyboard_P2", "K" },
            { "Xbox", "Y" },
            { "PlayStation", "▲" } // Triangle button
        }},
        { "UseItem3", new Dictionary<string, string> {
            { "Keyboard_P1", "E" },
            { "Keyboard_P2", "L" },
            { "Xbox", "B" },
            { "PlayStation", "●" } // Circle button
        }},
    };
}

