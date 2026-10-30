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

	public void Init(Transform toTrack)
	{
		target = toTrack;
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
