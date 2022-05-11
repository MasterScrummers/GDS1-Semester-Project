using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class TutorialDamage : MonoBehaviour
{
    void Start()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthComponent health = collision.GetComponent<HealthComponent>();
            health.TakeDamage(health.health - 1);
            gameObject.SetActive(false);
        }
    }
}
