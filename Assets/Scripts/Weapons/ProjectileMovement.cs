using UnityEngine;

public class ProjectileMovement : MonoBehaviour
{
    [SerializeField] protected float speed; //The moving speed;
    [SerializeField] protected float lifeTime; //How long it exist
    protected float lifeTick; //The current lifetimer

    protected Rigidbody2D rb; //Rigidbody 2D

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        OnEnable();
    }

    protected virtual void Update()
    {
        rb.velocity = speed * transform.right; //Allow the object to move in the direction it is facing.
        gameObject.SetActive((lifeTick -= Time.deltaTime) > 0);
    }

    protected virtual void OnEnable()
    {
        lifeTick = lifeTime;
    }
}
