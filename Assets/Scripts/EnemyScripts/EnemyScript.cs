using System.Drawing;
using UnityEngine;
using static EnemyStatManager;

public class EnemyScript : MonoBehaviour
{
	// References
	Transform target;

	[Header("Stats")]
	Vector2 movementInput = Vector2.zero;
	Vector2 velocity = Vector2.zero;
	float acceleration;
	float maxSpeed;
	float damping = .8f;

	private EnemyStats enemyStats = new EnemyStats();

	public void Init(Transform toTrack, float points)
	{
		target = toTrack;
		Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
		enemyStats.Initialize(points, position);
		enemyStats.PrintStats();
		ApplyStats();
	}

	private void ApplyStats()
	{
		StatFloat scaleStat = enemyStats.GetStat(StatType.Scale) as StatFloat;
		float scale = scaleStat.statValue;
		transform.localScale = new Vector3(scale, scale, scale);

		maxSpeed = (enemyStats.GetStat(StatType.MaxSpeed) as StatFloat).statValue;
		acceleration = (enemyStats.GetStat(StatType.Acceleration) as StatFloat).statValue;
	}

	private void FixedUpdate()
	{
		movementInput = (target.position - transform.position).normalized;

		// Movement
		velocity *= damping;
		velocity += movementInput * acceleration * Time.fixedDeltaTime;

		Vector2 normalizedVelocity = velocity.normalized;
		if (velocity.magnitude > maxSpeed) { velocity = normalizedVelocity * maxSpeed; }

		transform.position += (Vector3)velocity;
	}
}
