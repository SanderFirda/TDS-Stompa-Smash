using UnityEngine;
using System.Collections.Generic;

public class CrateSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> cratePrefabs;
    [SerializeField] private float spawnInterval = 0.5f;
    [SerializeField] private int maxCrates = 10;
    [SerializeField] private Bounds spawnArea = new Bounds(Vector3.zero, new Vector3(10, 10, 0));
    [SerializeField] private List<GameObject> spawnedCrates;
    private float spawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval && spawnedCrates.Count < maxCrates)
        {
            SpawnCrate();
            spawnTimer = 0f;
        }
        foreach (var crate in spawnedCrates)
        {
            if(!spawnArea.Contains(crate.transform.position))
            {
                Destroy(crate);
                spawnedCrates.Remove(crate);
                break;
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
}