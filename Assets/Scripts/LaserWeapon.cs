using System.Linq.Expressions;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserWeapon : MonoBehaviour
{
    InputAction shoot;

    [SerializeField] private LineRenderer laserBeam;
    [SerializeField] private float laserRange = 10f;
    [SerializeField] private GameObject laserMuzzle;
    [SerializeField] private float pullForce = 10f;
    [SerializeField] private float stunDuration = 0.1f;
    [SerializeField] private float maxPullSpeed = 5f;

    private RaycastHit2D hit;
    private RaycastHit2D lastHit;
    private EnemyMove enemyMove;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shoot = InputSystem.actions.FindAction("Shoot");
        if (shoot == null)
        { 
            Debug.LogError("Shoot action not found in Input System.");
        }
        if (!(laserBeam = transform.GetComponentInChildren<LineRenderer>() as LineRenderer))
        {
            Debug.LogError("LaserWeapon script requires a LineRenderer component.");
        }
        if(!laserMuzzle)
        {
            Debug.LogError("LaserWeapon script requires a reference to the laser muzzle GameObject.");
        }
        laserBeam.SetPosition(0, laserMuzzle.transform.localPosition);
    }

    // Update is called once per frame
    void Update()
    {
    }
    void FixedUpdate()
    {
        if (shoot.IsPressed())
        {
            laserBeam.enabled = true;
            if(hit = Physics2D.Raycast(laserMuzzle.transform.position, (transform.rotation * Vector2.up), laserRange))
            {
                //if(hit != lastHit)
                {
                    enemyMove = hit.collider.GetComponent<EnemyMove>();
                    lastHit = hit;
                }
                if (enemyMove != null)
                {
                    Debug.Log("Stunned: " + hit.collider.name);
                    enemyMove.Stun(stunDuration);
                }


                laserBeam.SetPosition(1,(Vector2.up * hit.distance) + laserMuzzle.transform.localPosition.ConvertTo<Vector2>());
                Debug.DrawRay(laserMuzzle.transform.position, (transform.rotation * Vector2.up) * hit.distance, Color.red);
                Debug.Log("Hit: " + hit.collider.name);


                if (hit.rigidbody != null)
                {
                    if(hit.rigidbody.linearVelocity.magnitude < maxPullSpeed)
                    {
                        hit.rigidbody.AddForce((hit.transform.position - transform.position).normalized * -pullForce, ForceMode2D.Impulse);
                        hit.rigidbody.AddTorque((hit.transform.position - transform.position).normalized.x * -pullForce * Random.Range(-1f, 1f), ForceMode2D.Impulse);                
                    }
                }
            }
            else
            {
                Debug.DrawRay(laserMuzzle.transform.position, (transform.rotation * Vector2.up) * laserRange, Color.red);
                laserBeam.SetPosition(1, Vector2.up * laserRange);
                enemyMove = null;
            }
        }
        else laserBeam.enabled = false;


    }
}
