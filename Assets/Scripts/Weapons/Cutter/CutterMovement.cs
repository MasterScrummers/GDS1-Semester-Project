using UnityEngine;

public class CutterMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    private float lifeTime = 5.0f;
    HealthComponent hp;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        hp = gameObject.GetComponent<HealthComponent>();
        rb.AddForce(new Vector2(25f, 0f) , ForceMode2D.Impulse);
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
