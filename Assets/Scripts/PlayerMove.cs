using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMove : MonoBehaviour
{
    InputAction move; //This is the input action variable we will use to listen to our movement input.
                      //We will assign this variable to the "Move" action in the Input System in the Start() method.


    Vector2 moveV; //This is our movement vector.
                   //We don't strictly need to store a separate variable for this, but it makes the code easier to
                   //read and write if we don't have to call "move.ReadValue<Vector2>()" every time we want to use the movement input.

    InputAction look; //Input action for look.

    Vector2 lookV;
    Vector2 dir;

    Rigidbody2D rb; //Our Rigidbody2D component, which we will use to apply movement forces to the player GameObject.

    //These are the movement parameters for our player. We can adjust these in the Unity Inspector to change how the player moves.
    [SerializeField] private float maxMoveSpeed = 5f;
    [SerializeField] private float moveForce = 10f;
    [SerializeField] private float rotationSpeed = 5f;

    //These are references to the player sprites, which we will rotate to face the direction of movement and aiming.
    [SerializeField] private GameObject bodySprite;
    [SerializeField] private GameObject legsSprite;

    //These are variables we will use to store the player's last position and the current aiming rotation, which we will use in our AnimateSprites() method to rotate the player sprites correctly.
    Vector2 lastPos;
    Quaternion aimQ;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        move = InputSystem.actions.FindAction("Move"); //Here we assign our movement input action to listen to the "Move" action
        look = InputSystem.actions.FindAction("Look"); //Here we assign our look input action.


        if (!(rb = transform.GetComponent<Rigidbody2D>())) //Here we try to get the Rigidbody2D component from the player GameObject, and if we can't find it, we log an error.
        {
            Debug.LogError("PlayerMove script requires a Rigidbody2D component.");
        }

        lastPos = transform.position; //We initialize our "lastPos" variable to the player's starting position, so that we can use it to calculate the movement direction in the first frame of the game.
    }

    // Update is called once per frame
    void Update()
    {
        AnimateSprites();
    }

    //FixedUpdate is called on a fixed time interval, and is where we want to put our movement code.
    private void FixedUpdate()
    {
        moveV = move.ReadValue<Vector2>(); //Here we read our "Move" input value. It is a Vector2, so we pass it to our "moveV" variable

        if (rb.linearVelocity.normalized.magnitude < maxMoveSpeed)
        {
              rb.AddForce(moveV * moveForce); //Here we use our "moveV" variable
        }

        //if (moveV != Vector2.zero) //If we have movement input, we want to rotate the player to face the direction of movement.
        //                           //If we don't have movement input, we want to keep the player facing the direction they were last moving in.
        //{
        //    aimQ = Quaternion.Euler(0, 0, Mathf.LerpAngle(bodySprite.transform.rotation.eulerAngles.z, Mathf.Atan2(moveV.y, moveV.x) * Mathf.Rad2Deg - 90, Time.deltaTime * rotationSpeed));
        //}
        aimQ = Quaternion.Euler(0, 0, GetLookRot());
    }

    private float GetLookRot()
    {
        lookV = look.ReadValue<Vector2>();

        if(lookV != Vector2.zero)
        {
            dir = lookV;
        }
        var device = look.activeControl?.device;

        if(device is Mouse)
        {
            dir = (Vector2)Camera.main.ScreenToWorldPoint((Vector3)lookV);
            dir = (dir - (Vector2)transform.position).normalized;

        }

        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90;
        return rot;
    }


    //LateUpdate is called after all Update and FixedUpdate calls have been made.
    private void LateUpdate()
    {
        lastPos = transform.position; //We do this in late update so that we store the position of the player after all movement has been applied in fixed update.

    }

    //This is just for the animation of the player sprites.
    //It rotates the legs to face the direction of movement, and the body to face the direction of aiming.
    void AnimateSprites()
    {
        Vector2 moveDir = (Vector2)transform.position - lastPos;

        if (moveDir.magnitude > 0.001f) //Here we rotate the legs sprite. We use a small threshold to avoid rotating the legs when there are some small floating point errors in the position.
        {
            float angle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            legsSprite.transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }

        bodySprite.transform.rotation = aimQ; //Here we rotate the body sprite to face the direction of aiming, which is stored in our "aimQ" variable, which is updated in our FixedUpdate() method to always face the direction of movement input.
    }
}
