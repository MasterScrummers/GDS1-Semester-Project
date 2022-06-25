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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            HealthComponent health = DoStatic.GetPlayer<HealthComponent>();
            health.SetHP(healthSet);
            Destroy(this);
        }
    }
}
