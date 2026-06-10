using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static int score = 0;

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
    }
    public static void AddScore(int value)
    {
        score += value;
        Debug.Log("Score: " + score);
    }
}