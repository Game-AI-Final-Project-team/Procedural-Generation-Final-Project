using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;

    float rotate = 1.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 distance = (player.GetComponent<Transform>().position - transform.position);
        float distanceValue = distance.magnitude;
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y) + (distance / distanceValue) * transform.localScale.x, (distance/distanceValue) * 3 + new Vector2(transform.position.x, transform.position.y) );

        if (hit)
        {
            if (hit.collider.gameObject.CompareTag("Wall"))
            {
                while (hit.collider.gameObject.CompareTag("Wall"))
                {
                    hit = Physics2D.Raycast((new Vector2(transform.position.x, transform.position.y) + (distance / distanceValue) * transform.localScale.x) * Mathf.Sin(rotate), ((distance / distanceValue) * 3 + new Vector2(transform.position.x, transform.position.y)) * Mathf.Sin(rotate));
                    
                    rotate *= -1.5f;
                    Debug.Log("hit wall");
                    if (rotate > 180)
                    {
                        break;
                    }
                }
                distance = (distance) * Mathf.Sin(rotate);
            }
            else
            {
                rotate = 1;
            }
        }

        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + (distance / distanceValue) * transform.localScale.x, (distance / distanceValue) * 3 + new Vector2(transform.position.x, transform.position.y), Color.yellow);

        if (distanceValue > 4)
        {
            rb.AddForce(distance);
            Debug.Log(distance);
        }
        if (distanceValue < 3)
        {
            rb.AddForce(-distance);
        }
    }
}
