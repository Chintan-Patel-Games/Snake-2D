using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private Snake1Controller snake1;
    [SerializeField] private Snake2Controller snake2;
    private Vector2 player1StartPos = Vector2.zero;
    private Vector2 player2StartPos = new Vector2(4,0);

    void Start()
    {
        // Check if multiplayer is enabled and instantiate Player 2 if necessary
        if (GameModeManager.Instance.IsMultiplayer)
        {
            player1StartPos = new Vector2(-4, 0);
            snake2.gameObject.SetActive(true);
            snake1.InitializeSnakeParts(player1StartPos);
            snake2.InitializeSnakeParts(player2StartPos);
        }
        else
        {
            snake2.gameObject.SetActive(false);
            snake1.InitializeSnakeParts(player1StartPos);
        }
    }
}