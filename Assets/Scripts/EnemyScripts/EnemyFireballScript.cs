using System.Collections;
using UnityEngine;

public class EnemyFireballScript : MonoBehaviour
{
    [SerializeField] float moveSpeed = 8;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] ParticleSystem fireballExplosion;

    private int damage;

    public void Init(Vector2 origin, int damage)
    {
        this.damage = damage;
        Debug.Log("My damage is:" + damage);
        Invoke(nameof(KillProjectile), 10);

        Vector2 direction = ((Vector2)transform.position - origin).normalized;
        rb.linearVelocity = direction * moveSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy")) return;

        if (other.CompareTag("Player") && other.TryGetComponent<IHasHealth>(out var player))
        {
            player.Hurt(damage);
            KillProjectile();
        }
        else if (other.CompareTag("Wall"))
        {
            KillProjectile();
        }
    }

    void KillProjectile()
    {
        if (gameObject != null && this != null)
        {
            if (fireballExplosion != null)
                Instantiate(fireballExplosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(Lifetime());
    }

    IEnumerator Lifetime()
    {
        yield return new WaitForSeconds(3);
        KillProjectile();
    }
}