using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance { get; private set; } // Singleton instance

    [Header("Single Player")]
    private int singlePlayerScore = 0;
    public int SinglePlayerScore { get { return singlePlayerScore; } }
    private int singlePlayerHighScore = 0;
    public int SinglePlayerHighScore { get { return singlePlayerHighScore; } }

    [Header("Multiplayer")]
    private int player1Score = 0;
    private int player2Score = 0;
    public int Player1Score { get { return player1Score; } }
    public int Player2Score { get { return player2Score; } }

    private int player1HighScore = 0;
    private int player2HighScore = 0;
    public int Player1HighScore { get { return player1HighScore; } }
    public int Player2HighScore { get { return player2HighScore; } }

    private int multiplier = 1;
    public int Multiplier { get; set; }

    // Events for single-player
    public delegate void SinglePlayerScoreUpdated(int score);
    public static event SinglePlayerScoreUpdated OnSinglePlayerScoreUpdated;

    public delegate void SinglePlayerHighScoreUpdated(int highScore);
    public static event SinglePlayerHighScoreUpdated OnSinglePlayerHighScoreUpdated;

    // Events for multiplayer
    public delegate void MultiplayerScoreUpdated(int player1Score, int player2Score);
    public static event MultiplayerScoreUpdated OnMultiplayerScoreUpdated;

    public delegate void MultiplayerHighScoreUpdated(int player1HighScore, int player2HighScore);
    public static event MultiplayerHighScoreUpdated OnMultiplayerHighScoreUpdated;

    private void Awake()
    {
        // Ensure only one instance of the ScoreController exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Load high scores from PlayerPrefs
        singlePlayerHighScore = PlayerPrefs.GetInt("SinglePlayerHighScore", 0);
        player1HighScore = PlayerPrefs.GetInt("Player1HighScore", 0);
        player2HighScore = PlayerPrefs.GetInt("Player2HighScore", 0);

        ResetSinglePlayerScore();
        ResetMultiplayerScores();
    }

    // Single Player Score Management
    public void AddSinglePlayerScore(int amount)
    {
        singlePlayerScore += amount * multiplier;

        if (singlePlayerScore > singlePlayerHighScore)
        {
            singlePlayerHighScore = singlePlayerScore;
            PlayerPrefs.SetInt("SinglePlayerHighScore", singlePlayerHighScore);
            PlayerPrefs.Save();
        }
        else if (singlePlayerScore <= 0) { singlePlayerScore = 0; }

        NotifySinglePlayerScoreUpdate();
    }

    public void ResetSinglePlayerScore()
    {
        singlePlayerScore = 0;
        NotifySinglePlayerScoreUpdate();
    }

    public void ResetSinglePlayerHighScore()
    {
        PlayerPrefs.SetInt("SinglePlayerHighScore", 0);
        PlayerPrefs.Save();
        singlePlayerHighScore = 0;
        NotifySinglePlayerScoreUpdate();
    }

    private void NotifySinglePlayerScoreUpdate()
    {
        OnSinglePlayerScoreUpdated?.Invoke(singlePlayerScore);
        OnSinglePlayerHighScoreUpdated?.Invoke(singlePlayerHighScore);
    }

    // Multiplayer Score Management
    public void AddPlayer1Score(int amount)
    {
        player1Score += amount * multiplier;

        if (player1Score > player1HighScore)
        {
            player1HighScore = player1Score;
            PlayerPrefs.SetInt("Player1HighScore", player1HighScore);
            PlayerPrefs.Save();
        }
        else if (player1HighScore <= 0) { player1HighScore = 0; }

        NotifyMultiplayerScoreUpdate();
    }

    public void AddPlayer2Score(int amount)
    {
        player2Score += amount * multiplier;

        if (player2Score > player2HighScore)
        {
            player2HighScore = player2Score;
            PlayerPrefs.SetInt("Player2HighScore", player2HighScore);
            PlayerPrefs.Save();
        }
        else if (player2Score <= 0) { player2Score = 0; }

        NotifyMultiplayerScoreUpdate();
    }

    public void ResetMultiplayerScores()
    {
        player1Score = 0;
        player2Score = 0;
        NotifyMultiplayerScoreUpdate();
    }

    public void ResetMultiplayerHighScores()
    {
        PlayerPrefs.SetInt("Player1HighScore", 0);
        PlayerPrefs.SetInt("Player2HighScore", 0);
        PlayerPrefs.Save();

        player1HighScore = 0;
        player2HighScore = 0;

        NotifyMultiplayerScoreUpdate();
    }

    private void NotifyMultiplayerScoreUpdate()
    {
        OnMultiplayerScoreUpdated?.Invoke(player1Score, player2Score);
        OnMultiplayerHighScoreUpdated?.Invoke(player1HighScore, player2HighScore);
    }
}