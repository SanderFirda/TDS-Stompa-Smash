using System;
using UnityEngine;
using UnityEngine.InputSystem;

/*
DESCRIPTION:
This script handles the 2D movement and visual orientation for a player character in Unity. 
It is designed for a top-down or side-scrolling game where the character's movement 
and its "aim" (where it is looking) can be independent.

How it works:

1. Input Handling:
   Uses Unity’s New Input System to listen for three specific actions:
   - Move: Captures a Vector2 for movement (like WASD or a joystick).
   - Look: Captures a Vector2 for looking (typically a joystick).
   - LookMouse: Captures the mouse position to determine where the player is aiming.

2. Physics-Based Movement:
   Movement is handled in FixedUpdate to ensure smooth interaction with the physics engine.
   It reads the movement input and applies a force to the character's Rigidbody2D.
   It includes a speed check: if the player is moving slower than maxMoveSpeed, 
   it continues to apply force; otherwise, it stops applying force to prevent infinite acceleration.

3. Look and Aim Logic:
   The script calculates the rotation for the player's "aim" using the GetLookRot() method.
   - Hybrid Input: It checks the GameManager to see if the user is using Mouse/Keyboard.
     If so, it converts the mouse's screen position into a world point and calculates 
     a direction vector relative to the player's position.
   - Rotation Calculation: It uses Mathf.Atan2 to convert the direction vector into 
     an angle (degrees), then offsets it by -90 degrees to align it correctly with the sprite's orientation.

4. Visual Animations (Split Orientation):
   One of the most important features is that it separates the body and the legs visually:
   - Legs: In AnimateSprites, the script compares the player's current position with 
     their previous position (lastPos). The legs rotate to face the direction the player is actually walking.
   - Body: The body sprite is rotated to face the "Look" direction (the direction of aiming).

Summary: This allows for "Twin-Stick" style movement, where a player can run in one 
direction (legs facing forward) while aiming their body in a different direction.
*/

public class PlayerMove : MonoBehaviour
{
    InputAction move;
    Vector2 moveV;

    InputAction look;
    InputAction lookMouse;

    Vector2 lookV;
    Vector2 dir;

    Rigidbody2D rb;

    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    [SerializeField] private GameObject bodySprite;
    [SerializeField] private GameObject legsSprite;

    Vector2 lastPos;
    Quaternion aimQ;

    void Start()
    {
        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        lookMouse = InputSystem.actions.FindAction("LookMouse");

        if (!(rb = transform.GetComponent<Rigidbody2D>()))
        {
            Debug.LogError("PlayerMove script requires a Rigidbody2D component.");
        }

        lastPos = transform.position;
    }

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

        aimQ = Quaternion.Euler(0, 0, GetLookRot());
    }

    private float GetLookRot()
    {
        lookV = look.ReadValue<Vector2>();
        if (lookV != Vector2.zero)
        {
            dir = lookV;
        }

        if (GameManager.inputType == GameManager.InputType.MouseKeyboard)
        {
            dir = (Vector2)Camera.main.ScreenToWorldPoint((Vector3)lookMouse.ReadValue<Vector2>());
            dir = (dir - (Vector2)transform.position).normalized;

        }
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        return rot;
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
