using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header ("Stats")]
    Vector2 movementInput = Vector2.zero;
    Vector2 velocity = Vector2.zero;
    [SerializeField] float acceleration = 5;
    [SerializeField] float maxSpeed = 20;
    [SerializeField] float damping = .8f;

    void Update()
    {
        // Movement
        movementInput = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            movementInput.y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
			movementInput.y -= 1;
		}
		if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A)) 
        {
			movementInput.x -= 1;
		}
		if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
			movementInput.x += 1;
		}
        movementInput.Normalize();

        // Rotation
        Vector3 mousePos = Input.mousePosition;
		Vector2 direction = mousePos - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.Euler(0, 0, angle);
	}

	private void FixedUpdate()
	{
        // Movement
        velocity *= damping;
        velocity += movementInput * acceleration * Time.fixedDeltaTime;
        transform.position += (Vector3)velocity;
	}
}
