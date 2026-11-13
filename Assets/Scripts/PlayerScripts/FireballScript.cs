using UnityEngine;

public class FireballScript : MonoBehaviour
{
	[SerializeField] float moveSpeed = 10;
	[SerializeField] int damage = 20;
	[SerializeField] Rigidbody2D rb;
	[SerializeField] ParticleSystem fireballExplosion;

	public void Init(Vector2 playerPos)
	{
		Invoke(nameof(KillProjectile), 10);

		Vector2 direction = ((Vector2)transform.position - playerPos).normalized;
		rb.linearVelocity = direction * moveSpeed;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.TryGetComponent<IHasHealth>(out var enemy))
		{
			enemy.Hurt(damage);
			KillProjectile();
		}
	}

	void KillProjectile() 
	{
		if (gameObject != null && this != null)
		{
			Instantiate(fireballExplosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
