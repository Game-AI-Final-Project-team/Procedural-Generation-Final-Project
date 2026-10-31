using UnityEngine;
using UnityEngine.UIElements;

public class EnemyBehavior : MonoBehaviour
{
    public GameObject player;
    public Rigidbody2D rb;

    float rotate = 0.0f;
    float destRotate = 0.0f;
    public float avoidance = 0.5f;
    Vector2 distance;
    float avTotal = 0.5f;

    [SerializeField] public LayerMask wall;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        distance = (player.GetComponent<Transform>().position - transform.position);
        if (avTotal>avoidance)
        {
            rotate = Mathf.Atan((distance.y / distance.x)) + avTotal;
            destRotate = Mathf.Atan(distance.y / distance.x);
        }
        else
        {
            rotate = Mathf.Atan(distance.y / distance.x);
            destRotate = Mathf.Atan(distance.y / distance.x);
        }

        fixRotation();

        float distanceValue = distance.magnitude;
        RaycastHit2D hitCurrent = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * 0.6f,
                    (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * 3 + new Vector2(transform.position.x, transform.position.y), wall);

        RaycastHit2D hitDest = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * 0.6f,
                    (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * 3 + new Vector2(transform.position.x, transform.position.y), wall);

        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * 0.6f,
                    (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * 3 + new Vector2(transform.position.x, transform.position.y), Color.yellow);

        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * 0.6f,
                    (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * 3 + new Vector2(transform.position.x, transform.position.y), Color.red);


        if (hitDest && hitDest.collider.gameObject.CompareTag("Wall"))
        {
            if (hitCurrent && hitCurrent.collider.gameObject.CompareTag("Wall"))
            {
                avTotal += avoidance;
                rotate += avoidance;
                fixRotation();
                
            }
        }
        else
        {
            avTotal = avoidance;
            rotate = Mathf.Atan(distance.y / distance.x);
            destRotate = Mathf.Atan(distance.y / distance.x);
            fixRotation();
        }




        distance = (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate))).normalized;

        if (distanceValue > 4)
        {
            rb.AddForce(distance*5);
        }
        if (distanceValue < 3)
        {
            rb.AddForce(-distance * 5);
        }

        RaycastHit2D circle = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y), 1.5f, new Vector2(0,0), 1.5f, wall);
        if (circle && circle.collider.CompareTag("Wall"))
        {
            Debug.Log("gottem");
            rb.AddForce((transform.position - circle.collider.GetComponent<Transform>().position));
        }
        
    }
    void fixRotation()
    {
        if (distance.x < 0)
        {
            rotate += 3.14159f;
            destRotate += 3.14159f;
        }
        if (rotate < 0)
        {
            rotate += 2 * (3.14159f);
        }
        if (rotate > 2 * 3.14159f)
        {
            rotate -= 2 * (3.14159f);
        }
        if (destRotate < 0)
        {
            destRotate += 2 * (3.14159f);
        }
        if (destRotate > 2 * 3.14159f)
        {
            destRotate -= 2 * (3.14159f);
        }
    }
}
