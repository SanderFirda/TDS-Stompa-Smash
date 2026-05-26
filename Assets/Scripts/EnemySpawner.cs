using UnityEngine;

public class EnemySpawner : MonoBehaviour
{

    [SerializeField] private GameObject enemyPrefab;

    [SerializeField] private int enemyCount = 5;
    [SerializeField] private float spawnRadius = 10f;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < enemyCount; i++)
        {
            Vector2 randomPoint = (Vector2)transform.position + Random.insideUnitCircle * spawnRadius;
            Instantiate(enemyPrefab, randomPoint, Quaternion.identity);
        }

    }
}
