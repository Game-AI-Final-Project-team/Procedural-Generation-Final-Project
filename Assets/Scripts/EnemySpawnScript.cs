using System.Collections;
using UnityEngine;

public class EnemySpawnScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] GameObject [] enemies;
    [SerializeField] float spawnTimer;
    [SerializeField] int maxEnemiesAtOnce;
    [SerializeField] int maxEnemiesTotal;
    private int enemiesSpawned = 0;
    [SerializeField] float pointsAllocated;
    private float pointsCurrentlyAllocated = 0;
    [Tooltip("This determines how many points an enemy can vary away from Points Allocated / Max Enemies.")]
    [SerializeField] float randomPointVariance;
    [HideInInspector] public GameObject[] spawnPositions;

	[Header("References")]
	[SerializeField] Transform player;

	private void Start()
    {
        spawnPositions = new GameObject[transform.childCount];
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
        Debug.Log(numEnemies + " this many enemies");
        return numEnemies;
    }

    public void SpawnEnemy()
    {
        if(CheckEnemies() < maxEnemiesAtOnce)
        {
            int i = Random.Range(0, enemies.Length);
            int j = Random.Range(0, spawnPositions.Length);
            Vector3 pos = spawnPositions[j].transform.position;
            EnemyScript eScript = Instantiate(enemies[i], spawnPositions[j].transform.position, Quaternion.identity).GetComponent<EnemyScript>();

            float pointsForEnemy = pointsAllocated / maxEnemiesTotal;
            pointsForEnemy += Random.Range(-randomPointVariance, randomPointVariance);
            pointsForEnemy = Mathf.Clamp(pointsForEnemy, (maxEnemiesTotal - enemiesSpawned) * (int)EnemyStatManager.StatType.LAST, pointsAllocated - (pointsCurrentlyAllocated + (maxEnemiesTotal - enemiesSpawned) * (int)EnemyStatManager.StatType.LAST));

            enemiesSpawned++;
            if (enemiesSpawned == maxEnemiesTotal)
            {
                pointsForEnemy = pointsAllocated - pointsCurrentlyAllocated;
            }

            pointsCurrentlyAllocated += pointsForEnemy;

            eScript.Init(player, pointsForEnemy);
        }
        if (enemiesSpawned >= maxEnemiesTotal) return;
        StartCoroutine(SpawnTime());
    }    

    IEnumerator SpawnTime()
    {
        yield return new WaitForSeconds(spawnTimer);
        SpawnEnemy();
    }
}
