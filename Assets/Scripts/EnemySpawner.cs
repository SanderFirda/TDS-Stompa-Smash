using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemyCount = 5;
    [SerializeField] private float spawnRadius = 10f;

    [SerializeField] private List<GameObject> spawnedEnemies;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 randomPoint = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            GameObject enemy = Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
            spawnedEnemies.Add(enemy);
        }

    }
    void Update()
    {
        for (int i = spawnedEnemies.Count - 1; i >= 0; i--)
        {
            if (spawnedEnemies[i] == null)
            {
                spawnedEnemies.RemoveAt(i);
            }
        }
        if (spawnedEnemies.Count == 0)
        {
            Debug.Log("All enemies defeated!");
        }
    }
}
