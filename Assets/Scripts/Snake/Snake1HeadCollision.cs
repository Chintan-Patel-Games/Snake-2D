using UnityEngine;

public class Snake1HeadCollision : MonoBehaviour
{
    private Snake1Controller snakeController; // Reference to the parent SnakeMovement script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        snakeController = FindObjectOfType<Snake1Controller>();
        if (snakeController != null)
        {
            snakeController.HandleCollision(collision); // Delegate collision handling to parent
        }
    }
}