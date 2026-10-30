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
        movementInput = Vector2.zero;
        if (Input.GetKey(KeyCode.UpArrow))
        {
            movementInput.y += 1;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
			movementInput.y -= 1;
		}
		if (Input.GetKey(KeyCode.LeftArrow))
        {
			movementInput.x -= 1;
		}
		if (Input.GetKey(KeyCode.RightArrow))
        {
			movementInput.x += 1;
		}
        movementInput.Normalize();
	}

	private void FixedUpdate()
	{
        velocity *= damping;
        velocity += movementInput * acceleration * Time.fixedDeltaTime;
        transform.position += (Vector3)velocity;
	}
}
