using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private List<Vector2> waypoints;

    private Vector2 targetWaypoint;

    private GameObject playerObject;

    private bool playerInRange = false;
    private bool runningFromPlayer = false;
    private bool leftPlayerRange = false;
    private float rangeTimer = 0f;


    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float wanderSpeed = 1f;
    [SerializeField] private float wanderRange = 2f;
    [SerializeField] private int wanderWaypoints = 5;
    [SerializeField] private float runAwaySpeed = 3f;
    [SerializeField] private float runAwayRange = 5f;
    [SerializeField] private float runAwayTime = 1f;

    private Vector2 destination;

    private float stunTime = 0;
    [SerializeField] private float maxStunTime = 1f;
    [SerializeField] private Vector2 waypointTimeOutRng = new Vector2(1f, 2f);
    private float wpointTimeOut;
    private float WPTimer = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!(playerObject = GameObject.FindGameObjectWithTag("Player")))
        {
            Debug.LogError("EnemyMove script requires a GameObject with the tag 'Player' in the scene.");
        }

        if (!(rb = transform.GetComponent<Rigidbody2D>()))
        {
            Debug.LogError("EnemyMove script requires a Rigidbody2D component.");
        }


        for (int i = 0; i < wanderWaypoints; i++)
        {
            Vector2 randomPoint = (Vector2)transform.position + Random.insideUnitCircle * wanderRange;
            waypoints.Add(randomPoint);
        }
        targetWaypoint = waypoints[0];
        wpointTimeOut = Random.Range(waypointTimeOutRng.x, waypointTimeOutRng.y);

    }

    // Update is called once per frame
    void Update()
    {
        if (stunTime > 0)
        {
            stunTime -= Time.deltaTime;
        }
        stunTime = Mathf.Clamp(stunTime, 0, maxStunTime);

        if (runningFromPlayer)
        {
            //Do stuff with animation?
        }
        if (leftPlayerRange)
        {
            rangeTimer += Time.deltaTime;
            if (rangeTimer >= runAwayTime)
            {
                FindNewWayPoint();
                playerInRange = false;
                runningFromPlayer = false;
                leftPlayerRange = false;
                rangeTimer = 0f;
            }
        }
    }

    private void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, targetWaypoint) < 0.1f)
        {
            FindNewWayPoint();
        }

        if (playerInRange)
        {
            RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, playerObject.transform.position, LayerMask.GetMask("SolidWalls"));
            if (hits.Length == 0)
            {
                runningFromPlayer = true;

                Vector2 directionAwayFromPlayer = -(playerObject.transform.position - transform.position).normalized;
                MoveTowardsTarget((Vector2)playerObject.transform.position + directionAwayFromPlayer * runAwayRange, runAwaySpeed);
                
                Debug.DrawLine(transform.position, playerObject.transform.position, Color.green);

                return;
            }
            else
            {
                runningFromPlayer = false;

                //Debug.Log("Hits: " + hits.Length);
                //Debug.Log(hits[0].collider.name);
                Debug.DrawLine(transform.position, playerObject.transform.position, Color.red);
            }
        }
        WPTimer += Time.deltaTime;
        if (WPTimer >= wpointTimeOut)
        {
            FindNewWayPoint();
            WPTimer = 0f;
        }

        MoveTowardsTarget(targetWaypoint, wanderSpeed);
        Debug.DrawLine(transform.position, targetWaypoint, Color.yellow);
        
    }
    void FindNewWayPoint()
    {
        targetWaypoint = waypoints[Random.Range(0, waypoints.Count)];
    }
    void MoveTowardsTarget(Vector2 target, float moveSpeed)
    {
        if (stunTime > 0)
        {
            return;
        }
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        rb.linearVelocity = direction * moveSpeed;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0, 0, (Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg) - 90), 0.3f);

        destination = target;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            leftPlayerRange = false;
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInRange = true;
            leftPlayerRange = false;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            leftPlayerRange = true;
        }
    }

    private void OnDrawGizmos()
    {
        foreach(Vector2 waypoint in waypoints)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(waypoint, 0.1f);
        }
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(destination, 0.1f);
    }

    public void Stun(float time)
    {
        stunTime += time;
    }
}
