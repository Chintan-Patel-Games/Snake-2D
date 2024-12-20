using UnityEngine;

public class Snake2HeadCollision : MonoBehaviour
{
    private Snake2Controller snakeController; // Reference to the parent SnakeMovement script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        snakeController = FindObjectOfType<Snake2Controller>();
        if (snakeController != null)
        {
            snakeController.HandleCollision(collision); // Delegate collision handling to parent
        }
    }
}