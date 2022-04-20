using UnityEngine;

public class CutterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private Vector2 velocity;
    private float lifeTime = 5.0f;
    HealthComponent hp;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        velocity = new Vector2(Random.Range(5f, 8f), Random.Range(0f, 5f));
        hp = gameObject.GetComponent<HealthComponent>();
        rb.AddForce(velocity , ForceMode2D.Impulse);
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0 || hp.health <= 0)
        {
            Destroy(gameObject);
        }

    }
}
