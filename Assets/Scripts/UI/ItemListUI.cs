using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using UnityEngine.InputSystem;  // For InputSystem namespace

public class ItemListUI : MonoBehaviour
{
    private PlayerInteraction playerInteraction;
    public Image itemImage1;
    public Image itemImage2;
    public Image itemImage3;
    public TextMeshProUGUI itemText1;
    public TextMeshProUGUI itemText2;
    public TextMeshProUGUI itemText3;
    private Image[] itemImages;

    public int index; // 0 or 1

    private List<ItemSO> lastItems = new List<ItemSO>();

    // Reference to PlayerInput component (automatically assigned)
    public PlayerInput playerInput;

    public void FindPlayer()
    {
        Player[] players = FindObjectsOfType<Player>();
        foreach (Player p in players)
        {
            if (p.playerID == index)
            {
                playerInteraction = p.GetComponent<PlayerInteraction>();
                playerInput = p.GetComponent<PlayerInput>();  // Get PlayerInput from player GameObject
                break;
            }
        }

        if (playerInteraction == null)
        {
            Debug.LogError("PlayerInteraction with playerID " + index + " not found.");
        }
    }

    public void UpdateUI()
    {
        if (playerInteraction == null || playerInput == null) return;

        List<ItemSO> items = playerInteraction.items;

        // Update item images
        for (int i = 0; i < itemImages.Length; i++)
        {
            if (i < items.Count && items[i] != null)
            {
                itemImages[i].sprite = items[i].icon;
                itemImages[i].enabled = true;
            }
            else
            {
                itemImages[i].enabled = false;
            }
        }

        // Update item action texts (key mapping)
        TextMeshProUGUI[] itemTexts = { itemText1, itemText2, itemText3 };

        // Check if the device is a keyboard or controller
        if (playerInput.currentControlScheme == "Keyboard&Mouse")
        {
            // Hardcoded keys for Keyboard
            string[] keyboardKeys = { "1", "2", "3" };

            for (int i = 0; i < 3; i++)
            {
                if (i < itemTexts.Length)
                {
                    itemTexts[i].text = keyboardKeys[i];  // Set text to 1, 2, 3 for keyboard
                    itemTexts[i].enabled = true;
                }
            }
        }
        else if (playerInput.currentControlScheme == "Gamepad")
        {
            // Hardcoded buttons for Controller
            string[] controllerButtons = { "X", "Y", "B" };

            for (int i = 0; i < 3; i++)
            {
                if (i < itemTexts.Length)
                {
                    itemTexts[i].text = controllerButtons[i];  // Set text to X, Y, B for controller
                    itemTexts[i].enabled = true;
                }
            }
        }

        // Cache current items
        lastItems = new List<ItemSO>(items);
    }

    private bool HasInventoryChanged()
    {
        if (playerInteraction == null) return false;

        List<ItemSO> items = playerInteraction.items;

        if (items.Count != lastItems.Count) return true;

        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] != lastItems[i])
                return true;
        }

        return false;
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

    void Update()
    {
        if (playerInteraction == null)
        {
            FindPlayer();
            return;
        }

        if (HasInventoryChanged())
        {
            UpdateUI();
        }
    }
}
