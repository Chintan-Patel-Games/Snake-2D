using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyManager : MonoBehaviour
{
    [SerializeField] private Button playBtn;
    // [SerializeField] private Button singlePlayerbtn;
    // [SerializeField] private Button multiplayerBtn;
    [SerializeField] private Button quitBtn;

    private void Start()
    {
        // Assign button listeners
        playBtn.onClick.AddListener(StartGame);
        // multiplayerButton.onClick.AddListener(StartSinglePlayer);
        // multiplayerButton.onClick.AddListener(StartMultiplayer);
        quitBtn.onClick.AddListener(QuitGame);
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Scene1");
    }

    // private void StartSinglePlayer()
    // {
    //     SceneManager.LoadScene("Scene1");
    // }

    // private void StartMultiplayer()
    // {
    //     SceneManager.LoadScene("Scene2");
    // }

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