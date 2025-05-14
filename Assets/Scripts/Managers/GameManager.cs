using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager main { get; private set; }

    void Awake()
    {
        // Singleton setup
        if (main == null)
        {
            main = this;   
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    // need a VoidEventSO if we want other instances to reply on GameOverEvent to settle EndGame.
    public void OnGameEnd()
    {
        // clear items
        ItemSpawnManager itemSpawnManager = FindAnyObjectByType<ItemSpawnManager>();
        if (itemSpawnManager != null)
        {
            itemSpawnManager.StopSpawning();
            itemSpawnManager.ClearSpawnedItems();   
        }
        
        // clear items in inventory
        PlayerInteraction[] playerInteractions = FindObjectsByType<PlayerInteraction>(FindObjectsSortMode.None);
        foreach (var interaction in playerInteractions)
        {
            interaction.ClearItems();
        }
    }
    
    // other pause game or end game menu displays
}
