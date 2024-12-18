using UnityEngine;

public class SnakeHeadCollision : MonoBehaviour
{
    [System.NonSerialized] public SnakeMovement snakeMovement; // Reference to the parent SnakeMovement script

    private void OnTriggerEnter2D(Collider2D collision)
    {
        snakeMovement = FindObjectOfType<SnakeMovement>();
        if (snakeMovement != null)
        {
            snakeMovement.HandleCollision(collision); // Delegate collision handling to parent
        }
    }
}