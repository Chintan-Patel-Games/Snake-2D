using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Button singlePlayerBtn;
    [SerializeField] private Button multiplayerBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        // Assign button listeners
        singlePlayerBtn.onClick.AddListener(StartSinglePlayer);
        multiplayerBtn.onClick.AddListener(StartMultiplayer);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void StartSinglePlayer()
    {
        GameModeManager.Instance.SetGameMode(GameMode.SinglePlayer); // Set to singleplayer
        LoadScene();
    }

    private void StartMultiplayer()
    {
        GameModeManager.Instance.SetGameMode(GameMode.MultiPlayer); // Set to multiplayer
        LoadScene();
    }

    private void LoadScene() { SceneManager.LoadScene("Scene1"); }

    private void QuitGame()
    {
        #if UNITY_WEBGL
        // Show a native browser alert
        Application.ExternalEval("alert('Thank you for playing! Please close the browser tab to exit.');");
        #else
        Application.Quit();
        #endif
    }
}