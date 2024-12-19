using UnityEngine;

public class ScoreController : MonoBehaviour
{
    public static ScoreController Instance { get; private set; } // Singleton instance
    private int currentScore = 0;
    public int CurrentScore { get { return currentScore; } }
    private int highScore = 0;
    public int HighScore { get { return highScore; } }
    private int multiplier = 1;
    public int Multiplier { get; set; }
    public delegate void ScoreUpdated(int score);
    public static event ScoreUpdated OnScoreUpdated;
    public delegate void HighScoreUpdated(int highScore);
    public static event HighScoreUpdated OnHighScoreUpdated;

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
        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
        ResetScore();
        NotifyAllScoreUpdate();
    }

    public void AddScore(int amount)
    {
        currentScore += amount * multiplier;

        // Update high score if needed
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }
        else if (currentScore <= 0) { currentScore = 0; }

        NotifyAllScoreUpdate();
    }

    public void ResetScore()
    {
        currentScore = 0;
        NotifyAllScoreUpdate();
    }

    public void ResetHighScore()
    {
        PlayerPrefs.SetInt("HighScore", 0);
        PlayerPrefs.Save();
        highScore = 0;
        NotifyAllScoreUpdate();
    }

    private void NotifyAllScoreUpdate()
    {
        OnScoreUpdated?.Invoke(currentScore);
        OnHighScoreUpdated?.Invoke(highScore);
    }
}