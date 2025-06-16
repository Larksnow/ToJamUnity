using System;
using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TextMeshPro))]
[RequireComponent(typeof(PlayerInput))]
public class GameManager : MonoBehaviour
{
    public static GameManager main { get; private set; }
    
    [Header("UI")]
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private String restartText = "Press R to restart";
    [SerializeField] private Color textColor = Color.yellow;
    
    [Header("Events")]
    public int restartGameDelay = 3;
    public IntEventSO restartGameEvent; 

    private PlayerInput _playerInput;
    
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

    private void Start()
    {
        restartGameEvent = ScriptableObject.CreateInstance<IntEventSO>();
        textMesh = GetComponent<TextMeshPro>();
        _playerInput = GetComponent<PlayerInput>();
        _playerInput.actions["Restart"].Disable();

        if (textMesh != null)
        {
            textMesh.gameObject.SetActive(false);
        }
    }

    // need a VoidEventSO if we want other instances to reply on GameOverEvent to settle EndGame.
    public void OnGameEnd()
    {
        _playerInput.actions["Restart"].Enable();
        
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
    public void ShowRestartText()
    {
        textMesh.gameObject.SetActive(true);
        textMesh.text = $"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{restartText}</color>";
    }
    
    public void OnRestart()
    {
        restartGameEvent.RaiseEvent(restartGameDelay, this);
        Invoke(nameof(ReloadScene), restartGameDelay);
        StartCoroutine(ReloadScene());
    }

    IEnumerator ReloadScene()
    {
        int localCount = restartGameDelay;
        while (localCount >= 0)
        {
            textMesh.text = $"<color=#{ColorUtility.ToHtmlStringRGB(textColor)}>{localCount}</color>";
            localCount--;
            yield return new WaitForSeconds(1);
        }
        
        textMesh.gameObject.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
