using UnityEngine;
using System.Collections.Generic;

public class Spawner : MonoBehaviour
{
    [Header("Spawn Area Settings")]
    [SerializeField] private Bounds spawnArea = new Bounds(Vector3.zero, new Vector3(20, 10, 0));
    [SerializeField] private EdgeCollider2D edgeCollider;

    
    [Header("Crate Settings")]
    [SerializeField] private List<GameObject> cratePrefabs;
    // [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private int maxCrates = 10;
    [SerializeField] private List<GameObject> spawnedCrates;

    [Header("Barrel Settings")]
    [SerializeField] private List<GameObject> barrelPrefabs;
    // [SerializeField] private float barrelSpawnInterval = 1f;
    [SerializeField] private int maxBarrels = 5;
    [SerializeField] private List<GameObject> spawnedBarrels;

    [Header("Enemy Settings")]
    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemyCount = 5;

    [SerializeField] private List<GameObject> spawnedEnemies;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!(edgeCollider = GetComponentInChildren<EdgeCollider2D>()))
        {
            Debug.LogError("Spawner script requires an EdgeCollider2D component in its children.");
        }
        edgeCollider.points = new Vector2[]
        {
            new Vector2(spawnArea.min.x, spawnArea.min.y),
            new Vector2(spawnArea.max.x, spawnArea.min.y),
            new Vector2(spawnArea.max.x, spawnArea.max.y),
            new Vector2(spawnArea.min.x, spawnArea.max.y),
            new Vector2(spawnArea.min.x, spawnArea.min.y)
        };

        for (int i = 0; i < enemyCount; i++)
        {
            SpawnEnemy();
        }

        for (int i = 0; i < maxCrates; i++)
        {
            SpawnCrate();
        }

        for(int i = 0; i < maxBarrels; i++)
        {
            SpawnBarrel();
        }
    }

    // Update is called once per frame
    void Update()
    {
        CheckEnemies();
    }

    private void CheckEnemies()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
                if (spawnedEnemies.Count == 0)
                {
                    //Debug.Log("All enemies defeated!");
                    GameManager.gameState = GameManager.GameState.Victory;
                }
            }
            else if (!spawnArea.Contains(spawnedEnemies[i].transform.position))
            {
                Debug.Log("Enemy left spawn area, respawning...");
                Destroy(spawnedEnemies[i]);
                spawnedEnemies.RemoveAt(i);
                SpawnEnemy();
            }
        }
    }

    private void SpawnCrate()
    {
        if (cratePrefabs.Count == 0) return;

        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnArea.min.x, spawnArea.max.x),
            Random.Range(spawnArea.min.y, spawnArea.max.y)
        );

        GameObject cratePrefab = cratePrefabs[Random.Range(0, cratePrefabs.Count)];
        GameObject spawnedCrate = Instantiate(cratePrefab, spawnPosition, Quaternion.identity);
        spawnedCrates.Add(spawnedCrate);
    }

    private void SpawnBarrel()
    {
        if (barrelPrefabs.Count == 0) return;

        Vector2 spawnPosition = new Vector2(
            Random.Range(spawnArea.min.x, spawnArea.max.x),
            Random.Range(spawnArea.min.y, spawnArea.max.y)
        );

        GameObject barrelPrefab = barrelPrefabs[Random.Range(0, barrelPrefabs.Count)];
        GameObject spawnedBarrel = Instantiate(barrelPrefab, spawnPosition, Quaternion.identity);
        spawnedBarrels.Add(spawnedBarrel);
    }

    private void SpawnEnemy()
    {
        Vector2 randomPoint = new Vector2(
            Random.Range(spawnArea.min.x, spawnArea.max.x),
            Random.Range(spawnArea.min.y, spawnArea.max.y)
        );
        GameObject enemy = Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
        spawnedEnemies.Add(enemy);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(spawnArea.center, spawnArea.size);
    }
}