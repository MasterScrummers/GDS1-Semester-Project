using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    protected float speed; //The moving speed;
    protected float lifeTime; //How long it exist (Don't need this if we are using object pooling)
    protected Rigidbody2D rb; //Rigidbody 2D
    protected Collider2D col; //Colider 2D

    public ProjectileBase()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
    }

    public virtual void Update()
    {
        rb.velocity = (transform.eulerAngles.z / 360) * speed * transform.right; //Allow the object to move at it's facing direction
    }

    /// <summary>
    /// Used to destory a gameObject after it's life time is equals to zero
    /// </summary>
    /// <param name="lifeTime"></param>
    public void DestroySelf(float lifeTime)
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Used to set a gameObject to Active or Inactive (Not sure if needed in object pooling)
    /// </summary>
    /// <param name="state"></param>
    public void SetActive(bool state)
    {
        gameObject.SetActive(state);
    }

    /// <summary>
    /// Meant to be overridden for OnTriggerEnter Event
    /// </summary>
    /// <param name="collision"></param>
    public virtual void OnTriggerEnter2D(Collider2D collision) { }
}
