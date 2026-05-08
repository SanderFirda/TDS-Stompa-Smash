using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    InputAction move;
    
    Vector2 moveV;

    Rigidbody2D rb;

    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private GameObject bodySprite;
    [SerializeField] private GameObject legsSprite;

    Vector2 lastPos;
    Quaternion aimQ;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        if (!(rb = transform.GetComponent<Rigidbody2D>()))
        {
            Debug.LogError("PlayerMove script requires a Rigidbody2D component.");
        }
        lastPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        AnimateSprites();


    }

    private void FixedUpdate()
    {
        moveV = move.ReadValue<Vector2>();
        if (rb.linearVelocity.normalized.magnitude < maxMoveSpeed)
        {
              rb.AddForce(moveV * moveForce);
        }

        if (moveV != Vector2.zero)
        {
            aimQ = Quaternion.Euler(0, 0, Mathf.LerpAngle(bodySprite.transform.rotation.eulerAngles.z, Mathf.Atan2(moveV.y, moveV.x) * Mathf.Rad2Deg - 90, Time.deltaTime * rotationSpeed));
        }
    }
    private void LateUpdate()
    {
        lastPos = transform.position;

    }

    void AnimateSprites()
    {
        Vector2 moveDir = (Vector2)transform.position - lastPos;
        
        if (moveDir.magnitude > 0.001f)
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            legsSprite.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
        bodySprite.transform.rotation = aimQ;
    }
}
