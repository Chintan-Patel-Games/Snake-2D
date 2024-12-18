using UnityEngine;

public class SnakeHeadCollision : MonoBehaviour
{
    private SnakeController snakeController; // Reference to the parent SnakeMovement script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        snakeController = FindObjectOfType<SnakeController>();
        if (snakeController != null)
        {
            snakeController.HandleCollision(collision); // Delegate collision handling to parent
        }
    }
}