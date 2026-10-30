using UnityEngine;

public class TrackingScript : MonoBehaviour
{
	[SerializeField] Transform trans;

	private void LateUpdate()
	{
		transform.position = trans.position;
		transform.position = new Vector3(transform.position.x, transform.position.y, -10);
	}
}
