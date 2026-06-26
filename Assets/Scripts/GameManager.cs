using UnityEngine;
using UnityEngine.InputSystem;


public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int score = 0;

    InputAction move;
    InputAction look;
    InputAction lookMouse;
    InputAction smash;
    InputAction shoot;

    public enum InputType { MouseKeyboard, Gamepad }
    public static InputType inputType;

    public enum GameState { MainMenu, Playing, Victory, GameOver }
    public static GameState gameState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        gameState = GameState.MainMenu;

        move = InputSystem.actions.FindAction("Move");
        look = InputSystem.actions.FindAction("Look");
        lookMouse = InputSystem.actions.FindAction("LookMouse");
        smash = InputSystem.actions.FindAction("Smash");
        shoot = InputSystem.actions.FindAction("Shoot");
    }
    public static void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }

    private void Update()
    {
        var device = smash.activeControl?.device;
        if(device != null)
        {
            if (device is Mouse)
            {
                inputType = InputType.MouseKeyboard;
            }
            else
            {
                inputType = InputType.Gamepad;
            }
        }
    }
}