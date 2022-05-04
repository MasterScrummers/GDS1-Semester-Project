using UnityEngine;

public class EnergyPulseMovement : MonoBehaviour
{
    private Animator anim; //The animation of the enegy pulse
    private Rigidbody2D rb; //The rigidbody of the gameobject
    [SerializeField] private Vector2 speed; //The speed of the energy pulse.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
        speed *= DoStatic.GetPlayer().transform.eulerAngles.y == 0 ? 1 : -1;
    }

    private void Update()
    {
        rb.velocity = speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HealthComponent>() && !collision.CompareTag("Player"))
        {
            anim.SetTrigger("End");
        }
    }
}
