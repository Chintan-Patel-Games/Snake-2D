using UnityEngine;

public class GameModeManager : MonoBehaviour
{
    public static GameModeManager Instance { get; private set; } // Singleton instance
    public bool IsMultiplayer { get; set; }

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

    public void SetGameMode(bool multiplayer)
    {
        IsMultiplayer = multiplayer;
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