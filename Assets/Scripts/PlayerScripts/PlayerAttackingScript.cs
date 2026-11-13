using UnityEngine;

public class PlayerAttackingScript : MonoBehaviour
{
	[SerializeField] GameObject fireballPrefab;
	bool attacking = false;

	private void Update()
	{
		if (Input.GetMouseButtonDown(0)) attacking = true;
		else if (Input.GetMouseButtonUp(0)) attacking = false; 
	}

	private void FixedUpdate()
	{
		if(attacking)
		{
			attacking = false;

			Vector3 spawnPos = transform.position + transform.right * 1;
			GameObject fireball = Instantiate(fireballPrefab, spawnPos, Quaternion.identity);
			if (fireball.TryGetComponent<FireballScript>(out var fScript)) fScript.Init(transform.position);
		}
	}
}
