using System.Drawing;
using UnityEngine;

public class EnemyScript : MonoBehaviour
{
	// References
	Transform target;

	[Header("Stats")]
	Vector2 movementInput = Vector2.zero;
	Vector2 velocity = Vector2.zero;
	[SerializeField] float acceleration = 5;
	[SerializeField] float maxSpeed = 20;
	[SerializeField] float damping = .8f;

	private EnemyStats enemyStats = new EnemyStats();

	public void Init(Transform toTrack, float points)
	{
		target = toTrack;
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
		enemyStats.Initialize(points, position);
	}

	private void FixedUpdate()
	{
		movementInput = (target.position - transform.position).normalized;

		// Movement
		velocity *= damping;
		velocity += movementInput * acceleration * Time.fixedDeltaTime;
		transform.position += (Vector3)velocity;
	}
}
