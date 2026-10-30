using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject [] enemies;
    [SerializeField] float spawnTimer;
    [SerializeField] int maxEnemies;

    private void Start()
    {
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
            Instantiate(enemies[i]);
        }
        StartCoroutine(SpawnTime());
    }    

    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(spawnTimer);
        SpawnEnemy();
    }
}
