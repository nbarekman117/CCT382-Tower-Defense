using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Spawner spawner;
    public HealthSystem health;
    public CurrencySystem currency;
    public List<Tower> activeTowers = new List<Tower>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
    void Start()
    {
        health.Init();
        currency.Init();
        EnemySpawner.instance.StartSpawning(); // Trigger enemy spawning
        StartCoroutine(WaveStartDelay());
    }

    IEnumerator WaveStartDelay()
    {
        yield return new WaitForSeconds(2f);
    }
}
