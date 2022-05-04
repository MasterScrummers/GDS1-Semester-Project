using UnityEngine;

public class CutterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float lifeTime = 5.0f;
    HealthComponent hp;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        hp = GetComponent<HealthComponent>();
        velocity = new Vector2(Random.Range(-5f, 8f), Random.Range(-5f, 5f));
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(collision.name);
    }
}
