using System.Collections;
using TMPro;
using UnityEngine;
using static EnemyStatManager;

public class EnemyCombatScript : MonoBehaviour, IHasHealth
{
    [Header("UI")]
    [SerializeField] TextMeshPro healthText;
    [Header("Shooting")]
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] float fireRate = 10f;
    [SerializeField] float range = 10f;
    private bool canShoot = true;

    private Transform player;
    private int damage;
    private int maxHealth;
    private int currentHealth;

    public int MaxHealth => maxHealth;
    public int CurrentHealth => currentHealth;

    public void Setup(Transform playerTransform, EnemyStats stats)
    {
        player = playerTransform;

        maxHealth = ((StatInt)stats.GetStat(StatType.Health)).statValue;
        currentHealth = maxHealth;
        healthText.text = $"{currentHealth}";

        damage = ((StatInt)stats.GetStat(StatType.Damage)).statValue;
    }

    void Update()
    {
        if (player == null) return;

        if (!canShoot) return;

        Vector3 toPlayer = player.position - transform.position;
        if (toPlayer.magnitude > range) return;

        Shoot(toPlayer.normalized);
    }

    void Shoot(Vector3 dir)
    {
        Vector3 spawnPos = transform.position + dir * 1;
        GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
        if (fireball.TryGetComponent<EnemyFireballScript>(out var fScript))
            fScript.Init(transform.position, damage);
        canShoot = false;
        StartCoroutine(ShotCooldown());
    }

    IEnumerator ShotCooldown()
    {
        yield return new WaitForSeconds(fireRate);
        canShoot = true;
    }

    public void Hurt(int damage)
    {
        if (currentHealth <= 0) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        
        healthText.text = $"{currentHealth}";
        
        if (currentHealth == 0)
            Die();
    }

    void Die()
    {
        Destroy(gameObject);
    }
}