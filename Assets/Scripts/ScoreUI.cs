using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;


    // Update is called once per frame
    void Update()
    {
        //De UI kan de score van de GameManager uitlezen
        scoreText.text = "Score: " + GameManager.score;

    }
}
