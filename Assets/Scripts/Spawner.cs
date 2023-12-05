using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{

    public GameObject ObjectToSpawn; // List of things to spawn
    public float SpawnDelay; // Time interval between spawns
    public float SpawnDistance; // Distance from the player to spawn item
    public float WaveDelay; // delay between waves. 
    public int[] ItemCounts; // list of interval amounts of items to spawn


    private int currentInterval = 0;
    private int itemsSpawnedInCurrentInterval = 0;
    private GameObject player;


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Finds the player
        StartCoroutine(SpawnItems());
    }


    IEnumerator SpawnItems()
    {
        while (currentInterval < ItemCounts.Length)
        {
            for (int i = 0; i < ItemCounts[currentInterval]; i++)
            {
                SpawnItem();
                yield return new WaitForSeconds(SpawnDelay); // Wait time between each enemy spawn
            }

            currentInterval++;
            yield return new WaitForSeconds(WaveDelay);
        }
    }

    void SpawnItem()
    {
        Vector3 spawnPosition = GetSpawnPosition();
        Instantiate(ObjectToSpawn, spawnPosition, Quaternion.identity);
    }

    Vector3 GetSpawnPosition()
    {
        if (player == null)
        {
            return Vector3.zero;
        }

        // Get the camera's viewpoint boundary to ensure spawning off-screen
        Vector2 screenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
        float x = Random.Range(-screenBounds.x, screenBounds.x);
        float y = Random.Range(-screenBounds.y, screenBounds.y);

        // Randomize the spawn location around the player, but keep it off-screen
        Vector3 randomDirection = new Vector3(x, y, 0).normalized;
        Vector3 spawnPoint = player.transform.position + randomDirection * SpawnDistance;

        // Ensure the spawn point is off-screen
        spawnPoint.x = Mathf.Clamp(spawnPoint.x, -screenBounds.x, screenBounds.x);
        spawnPoint.y = Mathf.Clamp(spawnPoint.y, -screenBounds.y, screenBounds.y);

        return spawnPoint;
    }
}
