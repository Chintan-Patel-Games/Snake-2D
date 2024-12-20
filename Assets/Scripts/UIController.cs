using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; } // Singleton instance

    [Header("Canvas References")]
    [SerializeField] private GameObject singlePlayerCanvas;
    [SerializeField] private GameObject multiplayerCanvas;
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private GameObject pauseCanvas;

    [Header("Single Player UI")]
    [SerializeField] private TextMeshProUGUI singlePlayerScoreText;
    [SerializeField] private TextMeshProUGUI singlePlayerHighScoreText;

    [Header("Multiplayer UI")]
    [SerializeField] private TextMeshProUGUI player1ScoreText;
    [SerializeField] private TextMeshProUGUI player1HighScoreText;
    [SerializeField] private TextMeshProUGUI player2ScoreText;
    [SerializeField] private TextMeshProUGUI player2HighScoreText;

    private bool isGameOver = false;
    private bool isMultiplayer = false; // Track if the current mode is multiplayer

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
        // Subscribe to the ScoreController events
        ScoreController.OnSinglePlayerScoreUpdated += UpdateSinglePlayerScoreUI;
        ScoreController.OnSinglePlayerHighScoreUpdated += UpdateSinglePlayerHighScoreUI;
        ScoreController.OnMultiplayerScoreUpdated += UpdateMultiplayerScoreUI;
        ScoreController.OnMultiplayerHighScoreUpdated += UpdateMultiplayerHighScoreUI;
    }

    private void Start()
    {
        // Set SinglePlayer UI active by default
        ShowSinglePlayerUI();

        // Ensure Game Over and Pause canvases are hidden at start
        HideGameOverUI();
        HidePauseUI();

        // Initialize the score and high score display
        UpdateSinglePlayerScoreUI(ScoreController.Instance.SinglePlayerScore);
        UpdateSinglePlayerHighScoreUI(ScoreController.Instance.SinglePlayerHighScore);
    }

    private void Update()
    {
        // Restart game on Enter key press if the game is over
        if (isGameOver && Input.GetKeyDown(KeyCode.Return))
        {
            RetryGame();
        }

        // Press "R" to reset the high scores
        if (Input.GetKeyDown(KeyCode.R))
        {
            ScoreController.Instance.ResetSinglePlayerHighScore();
            ScoreController.Instance.ResetMultiplayerHighScores();
        }

        // Pause/unpause the game on Esc key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
    }

    private void OnDisable()
    {
        // Unsubscribe from the ScoreController events
        ScoreController.OnSinglePlayerScoreUpdated -= UpdateSinglePlayerScoreUI;
        ScoreController.OnSinglePlayerHighScoreUpdated -= UpdateSinglePlayerHighScoreUI;
        ScoreController.OnMultiplayerScoreUpdated -= UpdateMultiplayerScoreUI;
        ScoreController.OnMultiplayerHighScoreUpdated -= UpdateMultiplayerHighScoreUI;
    }

    private void UpdateSinglePlayerScoreUI(int score)
    {
        singlePlayerScoreText.text = $"Score\n{score}";
    }

    private void UpdateSinglePlayerHighScoreUI(int highScore)
    {
        singlePlayerHighScoreText.text = $"Highscore\n{highScore}";
    }

    private void UpdateMultiplayerScoreUI(int player1Score, int player2Score)
    {
        player1ScoreText.text = $"P1 Score\n{player1Score}";
        player2ScoreText.text = $"P2 Score\n{player2Score}";
    }

    private void UpdateMultiplayerHighScoreUI(int player1HighScore, int player2HighScore)
    {
        player1HighScoreText.text = $"P1 Highscore\n{player1HighScore}";
        player2HighScoreText.text = $"P2 Highscore\n{player2HighScore}";
    }

    public void ShowSinglePlayerUI()
    {
        isMultiplayer = false;
        singlePlayerCanvas.SetActive(true);
        multiplayerCanvas.SetActive(false);
    }

    public void ShowMultiplayerUI()
    {
        isMultiplayer = true;
        singlePlayerCanvas.SetActive(false);
        multiplayerCanvas.SetActive(true);
    }

    public void ShowGameOverUI()
    {
        gameOverCanvas.SetActive(true);
        isGameOver = true;
        Time.timeScale = 0; // Pause the game when Game Over UI is shown
    }

    public void HideGameOverUI()
    {
        gameOverCanvas.SetActive(false);
        isGameOver = false;
        Time.timeScale = 1; // Resume the game when Game Over UI is not shown
    }

    public void RetryGame()
    {
        Time.timeScale = 1; // Resume the game
        if (isMultiplayer)
        {
            ScoreController.Instance.ResetMultiplayerScores();
        }
        else
        {
            ScoreController.Instance.ResetSinglePlayerScore();
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        isGameOver = false; // Reset the game over state
        HideGameOverUI();
    }

    public void TogglePauseMenu()
    {
        if (isGameOver) return; // Prevent pause during game over

        if (!pauseCanvas.activeSelf)
        {
            ShowPauseUI();
        }
        else
        {
            HidePauseUI();
        }
    }

    public void ShowPauseUI()
    {
        pauseCanvas.SetActive(true);
        Time.timeScale = 0; // Pause the game
    }

    public void HidePauseUI()
    {
        pauseCanvas.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }

    public void QuitToMainMenu()
    {
        // Destroy all singleton instances
        Destroy(ScoreController.Instance.gameObject);
        Destroy(UIController.Instance.gameObject);
        Time.timeScale = 1; // Resume the game
        SceneManager.LoadScene("Main Menu");
    }
}