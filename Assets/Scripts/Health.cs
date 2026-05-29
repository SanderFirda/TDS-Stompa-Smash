using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] private int maxHealth = 100;
    private int currentHealth;
    public bool IsAlive => currentHealth > 0;
    private enum HealthType { Player, Enemy, Neutral }

    [SerializeField] private HealthType healthType;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Damage taken! Current health: " + currentHealth);
        DamageVFX(damage);
        if (currentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(int amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log("Healed! Current health: " + currentHealth);
    }

    private void DamageVFX(int dmg)
    {
        // Implement visual effects for taking damage here

    }

    private void Die()
    {
        // Handle death logic here
        switch (healthType)
        {
            case HealthType.Player:
                // Player-specific death logic
                Debug.Log("Player has died.");
                break;

            case HealthType.Enemy:
                // Enemy-specific death logic
                Debug.Log("Enemy has died.");
                Destroy(gameObject);
                break;

            case HealthType.Neutral:
                Debug.Log("Neutral object destroyed.");
                break;
        }
    }
}