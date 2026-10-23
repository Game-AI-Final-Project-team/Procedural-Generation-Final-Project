using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = (player.GetComponent<Transform>().position - transform.position);
        float distanceValue = distance.magnitude;
        if (distanceValue > 5)
        {
            rb.AddForce(distance);
        }
        if (distanceValue < 3)
        {
            rb.AddForce(-distance);
        }
    }
}
