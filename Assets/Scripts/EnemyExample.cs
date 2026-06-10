using UnityEngine;

public class EnemyExample : MonoBehaviour
{
    public int health = 10;
    public int pointValue = 3;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Bullet")
        {
            health--;
            if (health <= 0)
            {
                //Dit script kan gewoon bij de GameManager
                //Je hoeft geen GetComponent<GameManager>() te doen
                GameManager.AddScore(pointValue);

                Destroy(gameObject);
            }
        }
    }
}
