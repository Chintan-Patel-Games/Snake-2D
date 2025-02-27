using UnityEngine;

public enum GameMode
{
    SinglePlayer,
    MultiPlayer
}

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; } // Singleton instance
    public GameMode CurrentGameMode { get; private set; } = GameMode.SinglePlayer; // Default mode

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Prevent duplicate instances
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Persist across scenes
    }

    public void SetGameMode(GameMode mode)
    {
        CurrentGameMode = mode;
    }

    // Method to destroy the singleton instance
    public void DestroySingleton()
    {
        if (Instance == this)
        {
            Destroy(gameObject);
            Instance = null;
        }
    }
}