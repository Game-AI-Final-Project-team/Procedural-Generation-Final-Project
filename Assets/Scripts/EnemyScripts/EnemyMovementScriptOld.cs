using System;
using UnityEngine;
using static EnemyStatManager;

public class EnemyMovementScriptOld : MonoBehaviour
{
	// References
	Transform target;

	[Header("Movement Stats")]
	Vector2 movementInput = Vector2.zero;
	Vector2 velocity = Vector2.zero;
	float linearAcceleration;
	float maxLinearSpeed;
	float damping = .8f;

	[Header("Rotation Stats")]
	float rotationAcceleration = 3;
	float rotationVelocity = 0;
	float maxRotationSpeed = 5;

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

		maxLinearSpeed = (enemyStats.GetStat(StatType.MaxSpeed) as StatFloat).statValue;
		linearAcceleration = (enemyStats.GetStat(StatType.Acceleration) as StatFloat).statValue;
	}

	private void FixedUpdate()
	{
		Vector3 targetPos = target.position;

		// Movement
		movementInput = (targetPos - transform.position).normalized;

		velocity *= damping;
		velocity += movementInput * linearAcceleration * Time.fixedDeltaTime;

		Vector2 normalizedVelocity = velocity.normalized;
		if (velocity.magnitude > maxLinearSpeed) { velocity = normalizedVelocity * maxLinearSpeed; }

		transform.position += (Vector3)velocity;

		// Rotation
		Vector2 direction = targetPos - transform.position;

		
		double targetAngle = Math.Atan2(direction.y, direction.x) * 180.0 / Math.PI;
		double diff = DeltaAngle(transform.eulerAngles.z, targetAngle);

		if (Math.Abs(diff) > 0.5)
		{
			if (diff > 0)
			{
				if (rotationVelocity < 0) rotationVelocity = 0;
				rotationVelocity += rotationAcceleration * Time.fixedDeltaTime;
			}
			else
			{
				if (rotationVelocity > 0) rotationVelocity = 0;
				rotationVelocity -= rotationAcceleration * Time.fixedDeltaTime;
			}
		}

		if (Math.Abs(rotationVelocity) > maxRotationSpeed)
			rotationVelocity = Math.Sign(rotationVelocity) * maxRotationSpeed;

		transform.Rotate(0, 0, rotationVelocity);

        
    }

    static double DeltaAngle(double current, double target)
	{
		double diff = (target - current) % 360.0;  
		if (diff > 180.0) diff -= 360.0;
		if (diff < -180.0) diff += 360.0;
		return diff;   
	}

}

