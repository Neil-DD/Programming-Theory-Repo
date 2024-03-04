using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private float spawnRange = 9;

    private int m_waveNumber;
    public int WaveNumber => m_waveNumber;//ENCAPSULATION

    public GameObject[] powerupPrefabs;

    // Update is called once per frame
    void Update()
    {
        int enemyCount = FindObjectsOfType<Enemy>().Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(++m_waveNumber);


            GameObject powerup = powerupPrefabs[Random.Range(0, powerupPrefabs.Length)];
            Instantiate(powerup, GenerateSpawnPosition(powerup), powerup.transform.rotation);
        }
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        if (enemiesToSpawn % 3 != 0)
        {
            for (int i = 0; i < enemiesToSpawn; i++)
            {
                GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length - 1)];
                Instantiate(enemyPrefab, GenerateSpawnPosition(enemyPrefab), enemyPrefab.transform.rotation);
            }
        }
        else//boss
        {
            GameObject enemyPrefab = enemyPrefabs[2];
            Instantiate(enemyPrefab, GenerateSpawnPosition(enemyPrefab), enemyPrefab.transform.rotation);
        }


    }

    public Vector3 GenerateSpawnPosition(GameObject enemyPrefab)
    {
        float spawnX = Random.Range(-spawnRange, spawnRange);
        float spawnZ = Random.Range(-spawnRange, spawnRange);

        Vector3 spawnPos = new Vector3(spawnX, enemyPrefab.transform.position.y, spawnZ);

        return spawnPos;
    }
}
