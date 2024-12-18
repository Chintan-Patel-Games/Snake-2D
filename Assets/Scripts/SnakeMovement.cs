using System.Collections.Generic;
using UnityEngine;

public class SnakeMovement : MonoBehaviour
{
    private List<Transform> snakeParts = new List<Transform>();
    private List<Vector2> previousPositions = new List<Vector2>();

    public GameObject snakeHeadPrefab;
    public GameObject snakeBodyPrefab;
    public GameObject snakeTailPrefab;

    public float moveSpeed = 0.2f;
    private float moveTimer = 0f;

    private Vector2 direction = Vector2.up;
    private Vector2 nextDirection;
    public int screenWidth = 10;
    public int screenHeight = 10;

    // Sprites for snake parts
    public Sprite headUp, headDown, headLeft, headRight;
    public Sprite tailUp, tailDown, tailLeft, tailRight;
    public Sprite bodyHorizontal, bodyVertical, bodyTopLeft, bodyTopRight, bodyDownLeft, bodyDownRight;

    void Start()
    {
        AddSnakePart(snakeHeadPrefab, Vector2.zero);      // Head
        AddSnakePart(snakeBodyPrefab, Vector2.down);      // Body
        AddSnakePart(snakeTailPrefab, 2 * Vector2.down);  // Tail

        // Initialize previous positions
        foreach (var part in snakeParts)
            previousPositions.Add(part.position);
    }

    void Update()
    {
        HandleInput();
    }

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

        // Move the head
        snakeParts[0].position += (Vector3)direction;

        // Screen wrapping (check if the snake crosses the boundaries)
        Vector2 headPosition = snakeParts[0].position;
        Vector2 wrappedPosition = headPosition;

        if (headPosition.x > screenWidth) wrappedPosition.x = -screenWidth;  // Wrap to left
        if (headPosition.x < -screenWidth) wrappedPosition.x = screenWidth; // Wrap to right
        if (headPosition.y > screenHeight) wrappedPosition.y = -screenHeight; // Wrap to bottom
        if (headPosition.y < -screenHeight) wrappedPosition.y = screenHeight; // Wrap to top

        // Apply the wrapped position to the head
        snakeParts[0].position = wrappedPosition;

        // Move the body parts to their previous part's position
        for (int i = 1; i < snakeParts.Count; i++)
        {
            snakeParts[i].position = previousPositions[i - 1];
        }

        // Update all sprites
        UpdateSprites();
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
        if (direction == Vector2.up) return headUp;
        if (direction == Vector2.down) return headDown;
        if (direction == Vector2.left) return headLeft;
        if (direction == Vector2.right) return headRight;
        return headUp;
    }

    private Sprite GetTailSprite(Vector2 tailDirection)
    {
        if (tailDirection == Vector2.up) return tailUp;
        if (tailDirection == Vector2.down) return tailDown;
        if (tailDirection == Vector2.left) return tailLeft;
        if (tailDirection == Vector2.right) return tailRight;
        return tailUp;
    }

    private Sprite GetBodySprite(Vector2 previousToCurrent, Vector2 currentToNext)
    {
        // Handle body parts with straight horizontal or vertical movement
        if (previousToCurrent == currentToNext)
        {
            // Horizontal body segment
            if (previousToCurrent == Vector2.left || previousToCurrent == Vector2.right)
                return bodyHorizontal;

            // Vertical body segment
            if (previousToCurrent == Vector2.up || previousToCurrent == Vector2.down)
                return bodyVertical;
        }

        // Handle the corner connections based on direction
        if (previousToCurrent == Vector2.up && currentToNext == Vector2.right) return bodyTopRight;
        if (previousToCurrent == Vector2.right && currentToNext == Vector2.down) return bodyTopLeft;

        if (previousToCurrent == Vector2.up && currentToNext == Vector2.left) return bodyTopLeft;
        if (previousToCurrent == Vector2.left && currentToNext == Vector2.down) return bodyTopRight;

        if (previousToCurrent == Vector2.down && currentToNext == Vector2.right) return bodyDownRight;
        if (previousToCurrent == Vector2.right && currentToNext == Vector2.up) return bodyDownLeft;

        if (previousToCurrent == Vector2.down && currentToNext == Vector2.left) return bodyDownLeft;
        if (previousToCurrent == Vector2.left && currentToNext == Vector2.up) return bodyDownRight;

        return bodyHorizontal; // Default fallback
    }

    private void AddSnakePart(GameObject prefab, Vector2 positionOffset)
    {
        GameObject part = Instantiate(prefab, (Vector2)transform.position + positionOffset, Quaternion.identity);
        part.transform.localScale = new Vector3(2.5f, 2.5f, 1);
        snakeParts.Add(part.transform);
        previousPositions.Add(part.transform.position);
    }
}