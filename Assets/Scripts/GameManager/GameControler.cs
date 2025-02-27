using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private SnakeController playerOneSnake;
    [SerializeField] private SnakeController playerTwoSnake;
    private Vector2 player1StartPos = Vector2.zero;
    private Vector2 player2StartPos = new Vector2(4,0);

    void Start()
    {
        // Check if multiplayer is enabled and instantiate Player 2 if necessary
        if (GameModeManager.Instance.CurrentGameMode == GameMode.MultiPlayer)
        {
            player1StartPos = new Vector2(-4, 0);
            playerTwoSnake.gameObject.SetActive(true);
            playerOneSnake.InitializeSnakeParts(player1StartPos);
            playerTwoSnake.InitializeSnakeParts(player2StartPos);
        }
        else if (GameModeManager.Instance.CurrentGameMode == GameMode.SinglePlayer)
        {
            playerTwoSnake.gameObject.SetActive(false);
            playerOneSnake.InitializeSnakeParts(player1StartPos);
        }
    }
}