using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [Header("Projectile Movement Parameters"), SerializeField] protected bool destroyOnContact = false;
    [SerializeField] protected OriginalValue<float> speed = new(1); //The moving speed;
    [SerializeField] protected Timer lifeTime = new(3);

    protected Rigidbody2D rb; //Rigidbody 2D

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        OnEnable();
    }

    protected virtual void Update()
    {
        rb.velocity = speed.value * transform.right; //Allow the object to move in the direction it is facing.
        lifeTime.Update(Time.deltaTime);
        gameObject.SetActive(lifeTime.tick > 0);
    }

    protected virtual void OnEnable()
    {
        lifeTime.Reset();
        speed.Reset();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (destroyOnContact)
        {
            lifeTime.Finish();
        }
    }
}
