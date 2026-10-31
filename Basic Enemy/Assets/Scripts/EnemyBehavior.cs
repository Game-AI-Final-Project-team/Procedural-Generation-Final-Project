using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;

    float rotate = 0.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = (player.GetComponent<Transform>().position - transform.position);
        if (distance.x > 0)
        {
            distance.x *= -1;
        }
        rotate = Mathf.Atan(distance.y / distance.x);
        float distanceValue = distance.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y),
                    (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) + new Vector2(transform.position.x, transform.position.y));
        
        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                Debug.Log("hit wall");
                
                while (hit.collider.gameObject.CompareTag("Wall"))
                {
                    rotate += 1.5f;
                    
                }
            }
        }

        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y),
                    (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) + new Vector2(transform.position.x, transform.position.y), Color.yellow);

        distance = (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate))).normalized;

        if (distanceValue > 4)
        {
            rb.AddForce(distance*5);
        }
        if (distanceValue < 3)
        {
            rb.AddForce(-distance * 5);
        }
        
    }
}
