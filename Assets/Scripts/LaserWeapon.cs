using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class LaserWeapon : MonoBehaviour
{
    InputAction shoot;

    [SerializeField] private LineRenderer laserBeam;
    [SerializeField] private float laserRange = 10f;
    [SerializeField] private GameObject laserMuzzle;
    private RaycastHit2D hit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        shoot = InputSystem.actions.FindAction("Shoot");
        if(!(laserBeam = transform.GetComponentInChildren<LineRenderer>() as LineRenderer))
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
                laserBeam.SetPosition(1,Vector2.up * hit.distance);
                Debug.DrawRay(laserMuzzle.transform.position, (transform.rotation * Vector2.up) * hit.distance, Color.red);

            }
            else
            {
                Debug.DrawRay(laserMuzzle.transform.position, (transform.rotation * Vector2.up) * laserRange, Color.red);
                laserBeam.SetPosition(1, Vector2.up * laserRange);

            }
        }
        else laserBeam.enabled = false;
        
    }
}
