using System.Collections;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public GameObject foodPrefab;  // Food prefab to spawn
    private float spawnCooldown = 5f; // Time after food is eaten before respawning

    private void Start()
    {
        // Spawn initial food
        SpawnFood();
        StartCoroutine(RespawnFoodCooldown());
    }

    public void SpawnFood()
    {
        // Position for the food (you can adjust the spawning logic to fit your grid or random spawn)
        Vector2 spawnPosition = new Vector2(Random.Range(-10, 10), Random.Range(-10, 10));

        // Instantiate the food at a random position within a specified range
        GameObject food = Instantiate(foodPrefab, spawnPosition, Quaternion.identity);

        // Set food's tag to "Food"
        food.transform.SetParent(transform);
    }

    private IEnumerator RespawnFoodCooldown()
    {
        while (true)  // Infinite loop to keep spawning food
        {
            SpawnFood();

            // Wait for the cooldown period before checking again
            yield return new WaitForSeconds(spawnCooldown);
        }
    }
}
