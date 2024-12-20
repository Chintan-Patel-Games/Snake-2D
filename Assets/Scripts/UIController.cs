using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; } // Singleton instance
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI highScoreText;
    // [SerializeField] private GameObject gameOverPanel;
    // [SerializeField] private GameObject gamePausePanel;
    private bool isGameOver = false;

    private void Awake()
    {
        // Ensure only one instance of the UIController exists
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

    private void OnEnable()
    {
        // Subscribe to the ScoreController event
        ScoreController.OnScoreUpdated += UpdateScoreUI;
        ScoreController.OnHighScoreUpdated += UpdateHighScoreUI;
    }

    private void Start()
    {
        // Initialize the score and high score display at the start
        UpdateScoreUI(ScoreController.Instance.CurrentScore);
        UpdateHighScoreUI(ScoreController.Instance.HighScore);
        // gameOverPanel.SetActive(false);
        // pauseMenu.SetActive(false);
    }

    private void Update()
    {
        // Restart game on Enter key press if the game is over
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            RetryGame();
        }

        if (Input.GetKeyDown(KeyCode.R)) // Press "R" to reset the high score
        {
            ScoreController.Instance.ResetHighScore();
        }

        // Pause/unpause the game on Esc key press
        // if (Input.GetKeyDown(KeyCode.Escape))
        // {
        //     TogglePauseMenu();
        // }
    }

    private void OnDisable()
    {
        // Unsubscribe from the ScoreController event
        ScoreController.OnScoreUpdated -= UpdateScoreUI;
        ScoreController.OnHighScoreUpdated -= UpdateHighScoreUI;
    }

    private void UpdateScoreUI(int currentScore)
    {
        scoreText.text = $"Score\n{currentScore}";
    }

    private void UpdateHighScoreUI(int highScore)
    {
        highScoreText.text = $"Highscore\n{highScore}";
    }

    public void ShowGameOver()
    {
        // gameOverPanel.SetActive(true);
        Time.timeScale = 0; // Pause the game
        isGameOver = true;  // Mark the game as over
    }

    public void RetryGame()
    {
        Time.timeScale = 1; // Resume the game
        ScoreController.Instance.ResetScore();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false; // Reset the game over state
    }

    // public void TogglePauseMenu()
    // {
    //     if (isGameOver) return; // Prevent pause during game over

    //     gamePausePanel.SetActive(!gamePausePanel.activeSelf);
    //     Time.timeScale = gamePausePanel.activeSelf ? 0 : 1; // Pause or resume the game
    // }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("MainMenu");
    }
}