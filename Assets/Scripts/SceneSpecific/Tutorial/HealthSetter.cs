using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class HealthSetter : MonoBehaviour
{
    [SerializeField] private int healthSet = 1;

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            HealthComponent health = collision.GetComponent<HealthComponent>();
            health.SetHP(healthSet);
            enabled = false;
        }
    }
}
