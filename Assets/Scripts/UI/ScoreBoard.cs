using UnityEngine;
using TMPro;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard Instance { get; private set; }

    private int[] scores = new int[2];
    private TextMeshPro textMesh;

    public Color player0Color = Color.red; 
    public Color player1Color = Color.blue;
    
    [Header("EndConditions")]
    [SerializeField] private int winScore = 25;
    [SerializeField] private float gameOverDelay = 3f;

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        textMesh = GetComponent<TextMeshPro>();
        if (textMesh == null)
        {
            Debug.LogError("Scoreboard requires a TextMeshPro component.");
        }

        UpdateScoreDisplay();
    }

    public void AddScore(int playerID)
    {
        if (playerID < 0 || playerID > 1) return;

        // Revert the playerID to the opponent's ID
        int opponentID = (playerID == 0) ? 1 : 0;

        // Increment the score for the opponent
        scores[opponentID]++;
        
        UpdateScoreDisplay();
        CheckAudioStateTrigger();
    }

    private void UpdateScoreDisplay()
    {
        if (textMesh == null) return;

        string left = $"<color=#{ColorUtility.ToHtmlStringRGB(player0Color)}>{scores[0]}</color>";
        string right = $"<color=#{ColorUtility.ToHtmlStringRGB(player1Color)}>{scores[1]}</color>";
        textMesh.text = $"{left} / {right}";
    }

    private void CheckAudioStateTrigger()
    {
        int sum = scores[0] + scores[1];

        if (sum == 1)
        {
            AudioManager.main.SetState("Damage", "two");
        }
        else if (sum == 4)
        {
            AudioManager.main.SetState("Damage", "three");
        }else if (sum == 8)
        {
            AudioManager.main.SetState("Damage", "four");
        }else if (sum == 16)
        {
            AudioManager.main.SetState("Damage", "five");
        }else if (IsGameEnd())
        {
            GameManager.main.OnGameEnd();
            AudioManager.main.PostEvent("Play_PauseMusic");
            Invoke(nameof(GameOver), gameOverDelay);
        } 
    }

    private bool IsGameEnd()
    {
        foreach (var score in scores)
        {
            if (score >= winScore)
            {
                return true;
            }
        }

        return false;
    }

    private void GameOver()
    {
        if (textMesh != null)
        {
            // hardcoded to get winner index
            string winnerName = scores[0] >=  winScore ? "Red" : "Blue";
            Color winnerColor = scores[0] >=  winScore ? player0Color : player1Color;
            string text = $"<color=#{ColorUtility.ToHtmlStringRGB(winnerColor)}>{winnerName}</color>";
            textMesh.text = $"{text} won!";
        }
        
        AudioManager.main.PostEvent("Play_PlayerWins");
    }
}
