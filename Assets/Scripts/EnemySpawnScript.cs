using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject [] enemies;
    [SerializeField] float spawnTimer;
    [SerializeField] int maxEnemies;
    private GameObject[] spawnPositions;

    private void Start()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            spawnPositions[i] = transform.GetChild(i).gameObject;
        }
        SpawnEnemy();
    }

    public int CheckEnemies()
    {
        int numEnemies = 0;
        GameObject[] activeEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in activeEnemies)
            numEnemies++;
        Debug.Log(numEnemies);
        return numEnemies;
    }

    public void SpawnEnemy()
    {
        if(CheckEnemies() < maxEnemies)
        {
            int i = Random.Range(0, enemies.Length);
            int j = Random.Range(0, spawnPositions.Length);
            Instantiate(enemies[i], spawnPositions[j].transform);
        }
        StartCoroutine(SpawnTime());
    }    

    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(spawnTimer);
        SpawnEnemy();
    }
}
