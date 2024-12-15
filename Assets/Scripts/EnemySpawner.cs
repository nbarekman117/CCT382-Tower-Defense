using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance;

    [Header("Enemy Prefabs and Spawn Points")]
    public List<GameObject> prefabs; // List of enemy prefabs
    public List<Transform> spawnPoints; // Spawn points

    [Header("Wave Settings")]
    public float waveDuration = 30f; // Duration of each wave in seconds
    public float wave1SpawnInterval = 3f; // Spawn interval for wave 1
    public float wave2SpawnInterval = 2.5f; // Spawn interval for wave 2
    public float wave3SpawnInterval = 2f; // Spawn interval for wave 3 and beyond

    [Header("UI Elements")]
    public TextMeshProUGUI waveText; // Reference to the wave text UI element
    public float waveTextDuration = 3f; // How long the wave text stays visible

    private int currentWave = 1; // Track the current wave
    private bool isSpawning = true; // Control the spawning coroutine

    void Awake()
    {
        instance = this;
    }

    public void StartSpawning()
    {
        Debug.Log("Enemy spawning started!");
        StartCoroutine(WaveManager());
    }

    IEnumerator WaveManager()
    {
        while (true)
        {
            DisplayWaveText($"Wave {currentWave}");
            Debug.Log("Wave " + currentWave + " started!");
            StartCoroutine(SpawnEnemiesForWave());
            yield return new WaitForSeconds(waveDuration);
            currentWave++;
        }
    }

    IEnumerator SpawnEnemiesForWave()
    {
        while (isSpawning)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(GetSpawnIntervalForWave());
        }
    }

    void SpawnEnemy()
    {
        int randomSpawnPointID = Random.Range(0, spawnPoints.Count);
        GameObject enemyToSpawn = null;

        // Determine which enemies to spawn based on the wave
        if (currentWave == 1)
        {
            enemyToSpawn = prefabs[0]; // Spawn only enemy 1
        }
        else if (currentWave == 2)
        {
            // Randomly pick between enemy 1 and enemy 2
            int randomEnemyID = Random.Range(0, 2); // Randomly pick 0 or 1
            enemyToSpawn = prefabs[randomEnemyID];
        }
        else if (currentWave >= 3)
        {
            // Randomly pick between enemy 1, enemy 2, and enemy 3
            int randomEnemyID = Random.Range(0, 3); // Randomly pick 0, 1, or 2
            enemyToSpawn = prefabs[randomEnemyID];
        }

        if (enemyToSpawn != null)
        {
            Instantiate(enemyToSpawn, spawnPoints[randomSpawnPointID]);
        }
    }

    float GetSpawnIntervalForWave()
    {
        // Use inspector-configurable spawn intervals
        if (currentWave == 1)
        {
            return wave1SpawnInterval;
        }
        else if (currentWave == 2)
        {
            return wave2SpawnInterval;
        }
        else
        {
            return wave3SpawnInterval;
        }
    }

    void DisplayWaveText(string message)
    {
        if (waveText != null)
        {
            waveText.text = message;
            waveText.gameObject.SetActive(true);
            StartCoroutine(HideWaveTextAfterDelay());
        }
    }

    IEnumerator HideWaveTextAfterDelay()
    {
        yield return new WaitForSeconds(waveTextDuration);
        if (waveText != null)
        {
            waveText.gameObject.SetActive(false);
        }
    }
}
