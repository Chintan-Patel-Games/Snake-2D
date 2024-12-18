using UnityEngine;

public class Food : MonoBehaviour
{
    private SnakeMovement snakeMovement;  // Reference to the SnakeMovement script

    private void Start()
    {
        snakeMovement = FindObjectOfType<SnakeMovement>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object that triggered the event is the snake's head
        if (other.CompareTag("SnakeHead"))  // Compare to the snake's head tag
        {
            // Get the last snake part's position
            Vector2 tailPosition = snakeMovement.snakeParts[snakeMovement.snakeParts.Count - 1].position;

            // Grow the snake by adding a new part at the tail's position
            snakeMovement.AddSnakePart(snakeMovement.snakeBodyPrefab, tailPosition);

            // Destroy the food
            Destroy(gameObject);
        }
    }
}
