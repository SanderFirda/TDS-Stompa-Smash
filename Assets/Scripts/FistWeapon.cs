using UnityEngine;
using UnityEngine.InputSystem;
public class FistWeapon : MonoBehaviour
{
    InputAction smash;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        smash = InputSystem.actions.FindAction("Smash");
        if (smash == null)
        {
            Debug.LogError("Smash action not found in Input System.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (smash.WasPressedThisFrame())
        {
                Debug.Log("Smash!");
        }
    }
}
