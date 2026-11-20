using System;
using UnityEngine;
using static EnemyStatManager;

public class EnemyMovementScript : MonoBehaviour
{
    // References
    Transform target;

    [Header("Movement Stats")]
    Vector2 velocity = Vector2.zero;
    float linearAcceleration = 5;
    float maxLinearSpeed = 15;
    float damping = .8f;

    [Header("Rotation Stats")]
    float rotationAcceleration = 3;
    float rotationVelocity = 0;
    float maxRotationSpeed = 5;

    [Header("Stat Data")]
    [SerializeField] EnemyStatData scaleStatData;
    [SerializeField] EnemyStatData speedStatData;


    [HideInInspector] public EnemyStats enemyStats;

    public Rigidbody2D rb;
    public GameObject player;

    float rotate = 0.0f;
    float destRotate = 0.0f;
    public float wallRadius;

    bool triggered = false;

    public float allyRadius;
    Vector2 distance;
    public float avTotal;

    [SerializeField] public LayerMask wall;

    [SerializeField] public LayerMask enemy;

    public void Init(Transform toTrack, float points)
    {
        wallRadius = transform.localScale.x * wallRadius;
        avTotal = rotationVelocity;
        target = toTrack;
        Vector2Int position = new Vector2Int((int)transform.position.x, (int)transform.position.y);
        enemyStats = new EnemyStats();
        enemyStats.Initialize(points, position);
        player = GameObject.FindWithTag("Player");
        //enemyStats.PrintStats();
        ApplyStats();
        GetComponent<EnemyCombatScript>().Setup(player.transform, enemyStats);
    }

    private void ApplyStats()
    {
        StatFloat scaleStat = EnemyStatManager.GetStatData(scaleStatData.key) as StatFloat;
        float size = scaleStat.statValue;
        transform.localScale = new Vector3(size, size, size);

        StatFloat speedStat = EnemyStatManager.GetStatData(speedStatData.key) as StatFloat;
        maxLinearSpeed = speedStat.statValue;
        // linearAcceleration = (enemyStats.GetStat(StatType.Acceleration) as StatFloat).statValue;
    }

    private void FixedUpdate()
    {
        if (target == null) return;

        distance = (target.position - transform.position);
        if (avTotal != 0)
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
        RaycastHit2D hitCurrent = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * (transform.localScale.x / 1.5f),
                    (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * 5f + new Vector2(transform.position.x, transform.position.y));

        RaycastHit2D hitDest = Physics2D.Linecast(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * (transform.localScale.x / 1.5f),
                    (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * 5f + new Vector2(transform.position.x, transform.position.y));

        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * (transform.localScale.x / 1.5f),
                    (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate)).normalized) * 5f + new Vector2(transform.position.x, transform.position.y), Color.yellow);

        Debug.DrawLine(new Vector2(transform.position.x, transform.position.y) + (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * (transform.localScale.x / 1.5f),
                    (new Vector2(distanceValue * Mathf.Cos(destRotate), distanceValue * Mathf.Sin(destRotate)).normalized) * 5f + new Vector2(transform.position.x, transform.position.y), Color.red);


        if (hitDest && ((hitDest.collider.gameObject.CompareTag("Wall")) || (hitDest.collider.gameObject.CompareTag("Enemy"))))
        {
            if (hitCurrent && ((hitCurrent.collider.gameObject.CompareTag("Wall")) || (hitCurrent.collider.gameObject.CompareTag("Enemy"))))
            {
                if (rotationVelocity < maxRotationSpeed && rotationVelocity >= 0)
                {
                    rotationVelocity += rotationAcceleration;
                }
                if (rotationVelocity > -maxRotationSpeed && rotationVelocity <= 0)
                {
                    rotationVelocity -= rotationAcceleration;
                }

                avTotal += rotationVelocity * Time.deltaTime;

                rotate += rotationVelocity * Time.deltaTime;

                if (triggered)
                {
                    avTotal *= -1;
                    rotate *= -1;
                    rotationVelocity *= -1;
                    
                }

                fixRotation();

                if (avTotal > 3)
                {
                    rotate = destRotate;
                    rotationVelocity = -0.01f;
                    avTotal = rotationVelocity;
                    triggered = false;

                }
                if (avTotal < -3)
                {
                    rotate = destRotate;
                    rotationVelocity = 0.01f;
                    avTotal = rotationVelocity;
                    triggered = false;
                }

            }
            else if (hitCurrent && (!(hitCurrent.collider.gameObject.CompareTag("Wall")) && !(hitCurrent.collider.gameObject.CompareTag("Enemy"))))
            {
                triggered = true;
            }
        }
        else
        {
            rotationVelocity = 0;
            avTotal = rotationVelocity;
            rotate = Mathf.Atan(distance.y / distance.x);
            destRotate = Mathf.Atan(distance.y / distance.x);
            triggered = false;
            fixRotation();
        }


        distance = (new Vector2(distanceValue * Mathf.Cos(rotate), distanceValue * Mathf.Sin(rotate))).normalized;

        if (distanceValue > 6 || destRotate != rotate)
        {
            velocity *= damping;
            if (velocity.magnitude < maxLinearSpeed)
            {
                velocity += distance * linearAcceleration * Time.fixedDeltaTime * 1000;
            }
            else
            {
                velocity += distance * Time.fixedDeltaTime * 1000;
            }
            rb.AddForce(velocity);
        }
        if (distanceValue < 3 && destRotate == rotate)
        {
            velocity *= damping;
            if (velocity.magnitude < maxLinearSpeed)
            {
                velocity += -distance * linearAcceleration * Time.fixedDeltaTime * 1300;
            }
            else
            {
                velocity += -distance * Time.fixedDeltaTime * 1300;
            }

            rb.AddForce(velocity);
        }

        RaycastHit2D circle = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y), wallRadius, new Vector2(0, 0), wallRadius, wall);
        RaycastHit2D circle2 = Physics2D.CircleCast(new Vector2(transform.position.x, transform.position.y), allyRadius, new Vector2(0, 0), allyRadius, enemy);


        if (circle && circle.collider.CompareTag("Wall"))
        {

            rb.AddForce((transform.position - circle.collider.GetComponent<Transform>().position).normalized * linearAcceleration * 1500 * Time.fixedDeltaTime);
        }

        if (circle2 && circle2.collider.CompareTag("Enemy"))
        {
            if (circle2.collider.GetComponent<Transform>().position != transform.position)
            {
                float enDistance = ((2 / (transform.position - circle2.collider.GetComponent<Transform>().position).sqrMagnitude)) ;
                rb.AddForce(((transform.position - circle2.collider.GetComponent<Transform>().position) * enDistance) * linearAcceleration * 1000 * Time.fixedDeltaTime);
            }

        }
    }

    static double DeltaAngle(double current, double target)
    {
        double diff = (target - current) % 360.0;
        if (diff > 180.0) diff -= 360.0;
        if (diff < -180.0) diff += 360.0;
        return diff;
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

    float getRadian(float degree)
    {
        return (degree / 180) * Mathf.PI;
    }
}

