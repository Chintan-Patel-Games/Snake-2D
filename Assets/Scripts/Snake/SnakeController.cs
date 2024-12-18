using System.Collections.Generic;
using UnityEngine;
using static SnakePartSprites;

public class SnakeMovement : MonoBehaviour
{
    [SerializeField] private GameObject snakeHeadPrefab;
    [SerializeField] private GameObject snakeBodyPrefab;
    [SerializeField] private GameObject snakeTailPrefab;
    [SerializeField] private int screenWidth = 10;
    [SerializeField] private int screenHeight = 10;
    [SerializeField] private List<SnakePartSprite> snakePartSprites;
    [SerializeField] private float moveSpeed;
    private Dictionary<SnakeParts, Sprite> spriteMap;
    private GameObject snakeHeadInstance;
    private float moveTimer = 0f;
    private List<Transform> snakeParts = new List<Transform>();
    private List<Vector2> previousPositions = new List<Vector2>();
    private Vector2 direction = Vector2.up;
    private Vector2 nextDirection;

    void Start()
    {
        spriteMap = new Dictionary<SnakeParts, Sprite>(); // Initialize the dictionary for quick lookups

        foreach (var entry in snakePartSprites)
        {
            if (!spriteMap.ContainsKey(entry.part))
            {
                spriteMap.Add(entry.part, entry.sprite);
            }
        }

        AddSnakePart(snakeHeadPrefab, Vector2.zero);  // Head
        AddSnakePart(snakeBodyPrefab, Vector2.down);  // Body
        AddSnakePart(snakeTailPrefab, Vector2.down);  // Tail

        // Initialize previous positions
        foreach (var part in snakeParts)
            previousPositions.Add(part.position);
    }

    void Update() { HandleInput(); }

    void FixedUpdate()
    {
        moveTimer += Time.deltaTime;

        if (moveTimer >= moveSpeed)
        {
            moveTimer = 0f;
            MoveSnake();
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && direction != Vector2.down) nextDirection = Vector2.up;
        else if (Input.GetKeyDown(KeyCode.DownArrow) && direction != Vector2.up) nextDirection = Vector2.down;
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && direction != Vector2.right) nextDirection = Vector2.left;
        else if (Input.GetKeyDown(KeyCode.RightArrow) && direction != Vector2.left) nextDirection = Vector2.right;
    }

    private void MoveSnake()
    {
        // Update head direction
        if (nextDirection != Vector2.zero && nextDirection != -direction)
            direction = nextDirection;

        // Save previous positions
        for (int i = 0; i < snakeParts.Count; i++)
            previousPositions[i] = snakeParts[i].position;

        snakeParts[0].position += (Vector3)direction; // Move the head

        // Screen wrapping (check if the snake crosses the boundaries)
        Vector2 headPosition = snakeParts[0].position;
        Vector2 wrappedPosition = headPosition;

        if (headPosition.x > screenWidth) wrappedPosition.x = -screenWidth; // Wrap to left
        if (headPosition.x < -screenWidth) wrappedPosition.x = screenWidth; // Wrap to right
        if (headPosition.y > screenHeight) wrappedPosition.y = -screenHeight; // Wrap to bottom
        if (headPosition.y < -screenHeight) wrappedPosition.y = screenHeight; // Wrap to top

        snakeParts[0].position = wrappedPosition; // Apply the wrapped position to the head

        // Move the body parts to their previous part's position
        for (int i = 1; i < snakeParts.Count; i++)
        {
            snakeParts[i].position = previousPositions[i - 1];
        }

        UpdateSprites(); // Update all sprites
    }

    private void UpdateSprites()
    {
        // Update head sprite
        snakeParts[0].GetComponent<SpriteRenderer>().sprite = GetHeadSprite();

        // Update body sprites
        for (int i = 1; i < snakeParts.Count - 1; i++)
        {
            Vector2 previousToCurrent = (Vector2)snakeParts[i].position - (Vector2)snakeParts[i - 1].position;
            Vector2 currentToNext = (Vector2)snakeParts[i + 1].position - (Vector2)snakeParts[i].position;

            snakeParts[i].GetComponent<SpriteRenderer>().sprite = GetBodySprite(previousToCurrent, currentToNext);
        }

        // Update tail sprite
        Vector2 tailDirection = (Vector2)snakeParts[snakeParts.Count - 2].position - (Vector2)snakeParts[snakeParts.Count - 1].position;
        snakeParts[snakeParts.Count - 1].GetComponent<SpriteRenderer>().sprite = GetTailSprite(tailDirection);
    }

    private Sprite GetHeadSprite()
    {
        if (direction == Vector2.up) return spriteMap[SnakeParts.headUp];
        if (direction == Vector2.down) return spriteMap[SnakeParts.headDown];
        if (direction == Vector2.left) return spriteMap[SnakeParts.headLeft];
        if (direction == Vector2.right) return spriteMap[SnakeParts.headRight];
        return spriteMap[SnakeParts.headUp]; // Fallback
    }

    private Sprite GetTailSprite(Vector2 tailDirection)
    {
        if (tailDirection == Vector2.up) return spriteMap[SnakeParts.tailUp];
        if (tailDirection == Vector2.down) return spriteMap[SnakeParts.tailDown];
        if (tailDirection == Vector2.left) return spriteMap[SnakeParts.tailLeft];
        if (tailDirection == Vector2.right) return spriteMap[SnakeParts.tailRight];
        return spriteMap[SnakeParts.tailUp]; // Fallback
    }

    private Sprite GetBodySprite(Vector2 previousToCurrent, Vector2 currentToNext)
    {
        // Handle body parts with straight horizontal or vertical movement
        if (previousToCurrent == currentToNext)
        {
            // Horizontal body segment
            if (previousToCurrent == Vector2.left || previousToCurrent == Vector2.right)
                return spriteMap[SnakeParts.bodyHorizontal];

            // Vertical body segment
            if (previousToCurrent == Vector2.up || previousToCurrent == Vector2.down)
                return spriteMap[SnakeParts.bodyVertical];
        }

        // Corner segments
        if (previousToCurrent == Vector2.up && currentToNext == Vector2.right) return spriteMap[SnakeParts.bodyTopRight];
        if (previousToCurrent == Vector2.right && currentToNext == Vector2.down) return spriteMap[SnakeParts.bodyTopLeft];
        if (previousToCurrent == Vector2.up && currentToNext == Vector2.left) return spriteMap[SnakeParts.bodyTopLeft];
        if (previousToCurrent == Vector2.left && currentToNext == Vector2.down) return spriteMap[SnakeParts.bodyTopRight];
        if (previousToCurrent == Vector2.down && currentToNext == Vector2.right) return spriteMap[SnakeParts.bodyDownRight];
        if (previousToCurrent == Vector2.right && currentToNext == Vector2.up) return spriteMap[SnakeParts.bodyDownLeft];
        if (previousToCurrent == Vector2.down && currentToNext == Vector2.left) return spriteMap[SnakeParts.bodyDownLeft];
        if (previousToCurrent == Vector2.left && currentToNext == Vector2.up) return spriteMap[SnakeParts.bodyDownRight];

        return spriteMap[SnakeParts.bodyHorizontal]; // Fallback
    }

    public void HandleCollision(Collider2D collision)
    {
        if (collision.CompareTag("MassGainer"))
        {
            AddSnakePart(snakeBodyPrefab, -direction); // Increase the snake length
            Destroy(collision.gameObject); // Destroy the food
        }

        if (collision.CompareTag("SnakeBody") || collision.CompareTag("SnakeTail"))
        {
            Debug.Log("Game Over!");
            Time.timeScale = 0;
        }
    }

    public void AddSnakePart(GameObject prefab, Vector2 positionOffset)
    {
        // Determine the spawn position for the new snake part
        Vector2 spawnPosition;

        if (snakeParts.Count > 0)
        {
            // Use the position of the last snake part in the list (except the head)
            spawnPosition = (Vector2)snakeParts[0].position + positionOffset; // Use head's position for new parts
        }
        else
        {
            // Default to the snake's current position if no parts exist
            spawnPosition = (Vector2)transform.position + positionOffset;
        }

        // Instantiate the snake part at the determined position
        GameObject part = Instantiate(prefab, spawnPosition, Quaternion.identity);

        // Set the parent
        part.transform.SetParent(transform);
        part.transform.localScale = new Vector3(2.5f, 2.5f, 1);

        // Track the snake head specifically
        if (prefab == snakeHeadPrefab)
        {
            snakeHeadInstance = part;

            // Add head as the first part in the list
            snakeParts.Insert(0, part.transform);  // Insert at the beginning (head first)
            previousPositions.Insert(0, part.transform.position);  // Add head's position to the front
        }
        else
        {
            // For body and tail, insert them after the head
            snakeParts.Insert(1, part.transform);  // Insert after the head
            previousPositions.Insert(1, part.transform.position);  // Insert body/tail after the head
        }
    }
}