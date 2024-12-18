using UnityEngine;

public class SnakePartSprites : MonoBehaviour
{
    [System.Serializable]
    public class SnakePartSprite
    {
        public SnakeParts part; // Enum for the snake part
        public Sprite sprite;   // Sprite associated with this part
    }
}