using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropSpawner : MonoBehaviour
{
    [System.Serializable]
    public class Prop
    {
        public string name;               // Name of the prop
        public GameObject prefab;         // Prefab for the prop
        public float spawnInterval = 5f;  // Time interval for spawning the prop
        [HideInInspector] public GameObject currentInstance; // Tracks the spawned instance
    }

    [SerializeField] private List<Prop> props = new List<Prop>();  // List of all props to spawn
    [SerializeField] private Vector2Int spawnAreaMin = new Vector2Int(-10, -10);  // Bottom-left corner of the spawn area
    [SerializeField] private Vector2Int spawnAreaMax = new Vector2Int(10, 10);    // Top-right corner of the spawn area

    private void Start()
    {
        // Start spawning each prop with its own coroutine
        foreach (Prop prop in props)
        {
            StartCoroutine(HandlePropSpawning(prop));
        }
    }

    private IEnumerator HandlePropSpawning(Prop prop)
    {
        while (true)
        {
            // Wait until the prop has been "eaten" or interacted with
            while (prop.currentInstance != null)
            {
                yield return null;  // Wait for the next frame
            }

            // Wait for the specified spawn interval
            yield return new WaitForSeconds(prop.spawnInterval);

            // Spawn the prop
            SpawnProp(prop);
        }
    }

    private void SpawnProp(Prop prop)
    {
        // Generate a random position within the defined spawn area
        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnAreaMin.x, spawnAreaMax.x),
            Random.Range(spawnAreaMin.y, spawnAreaMax.y)
        );

        // Instantiate the prop prefab and track it
        prop.currentInstance = Instantiate(prop.prefab, spawnPosition, Quaternion.identity);

        // Set the spawned prop as a child of this spawner for organization
        prop.currentInstance.transform.SetParent(transform);

        // Destroy the prop after 10 seconds if it hasn't been interacted with
        Destroy(prop.currentInstance, 10f);
    }
}