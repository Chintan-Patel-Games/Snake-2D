using UnityEngine;

public class SnakeHeadCollision : MonoBehaviour
{
    private SnakeController snakeController; // Reference to the parent SnakeController script

    private void Start()
    {
        // Get the SnakeController component from the parent object
        snakeController = GetComponentInParent<SnakeController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (snakeController != null)
        {
            snakeController.HandleCollision(collision); // Delegate collision handling to the parent
        }
    }
}