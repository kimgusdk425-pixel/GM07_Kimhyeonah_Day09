using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [Header("Spawn Enemy Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Position")]
    [SerializeField] private Transform spawnPoint;

    [Header("Spawn Setting")]
    [SerializeField] private int spawnCount = 10;
    [SerializeField] private float spawnInterval = 1.0f;

    private WaitForSeconds waitSpawn;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waitSpawn = new WaitForSeconds(spawnInterval);
        StartCoroutine(spawnCo());
    }
    IEnumerator spawnCo()
    {
        for(int i = 0; i < spawnCount; i++)
        {
            Instantiate(enemyPrefab, spawnPoint.position, spawnPoint.rotation);
            yield return waitSpawn;
        }
    }
}
