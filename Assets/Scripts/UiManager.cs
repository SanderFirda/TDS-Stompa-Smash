using UnityEngine;
using static GameManager;

public class UiManager : MonoBehaviour
{

    [SerializeField] private GameObject gameOverMenu;
    [SerializeField] private GameObject victoryMenu;
    [SerializeField] private GameObject mainMenu;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameOverMenu.SetActive(false);
        victoryMenu.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (gameState)
        {
            case GameState.GameOver:
                mainMenu.SetActive(false);
                gameOverMenu.SetActive(true);
                victoryMenu.SetActive(false);
                break;

            case GameState.Victory:
                mainMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                victoryMenu.SetActive(true);
                break;

            case GameState.MainMenu:
                mainMenu.SetActive(true);
                gameOverMenu.SetActive(false);
                victoryMenu.SetActive(false);
                break;

            case GameState.Playing:
                mainMenu.SetActive(false);
                gameOverMenu.SetActive(false);
                victoryMenu.SetActive(false);
                break;
        }
    }

    public void RestartGame()
    {
        gameState = GameState.Playing;
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayGame()
    {
        gameState = GameState.Playing;
    }
}
