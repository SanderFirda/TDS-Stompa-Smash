using Unity.VisualScripting;
using UnityEngine;

public class ExplosiveBarrel : MonoBehaviour
{
    [SerializeField] private float explosionRadius = 3f;
    [SerializeField] private int explosionDamage = 50;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private Health health;
    private bool hasExploded = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(!(health = GetComponent<Health>()))
        {
            Debug.LogError("ExplosiveBarrel requires a Health component.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(health.IsAlive == false && !hasExploded)
        {
            Explode();
            hasExploded = true;

            Destroy(gameObject);
        }
    }

    private void Explode()
    {
            foreach (var collider in Physics2D.OverlapCircleAll(transform.position, explosionRadius))
            {
                var rb = collider.GetComponent<Rigidbody2D>();
                    if (rb != null)
                    {
                        Vector2 explosionDirection = (collider.transform.position - transform.position).normalized;
                        rb.AddForce(explosionDirection * explosionForce, ForceMode2D.Impulse);
                }
                var health = collider.GetComponent<Health>();
                if (health != null)
                {
                    health.TakeDamage(explosionDamage);
                }
            }
            if (explosionEffectPrefab != null)
            {
                GameObject exp = Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
                Destroy(exp, 3f);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
