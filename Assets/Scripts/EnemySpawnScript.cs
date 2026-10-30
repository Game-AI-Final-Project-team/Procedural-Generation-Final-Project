using System.Collections;
using System.Security.Cryptography;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject [] enemies;
    [SerializeField] float spawnTimer;
    [SerializeField] int maxEnemies;
    int numEnemies;

    private void Start()
    {
        SpawnEnemy();
    }

    public int CheckEnemies()
    {
        int numEnemies = 0;

        return numEnemies;
    }

    public void SpawnEnemy()
    {
        if(numEnemies != maxEnemies)
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
