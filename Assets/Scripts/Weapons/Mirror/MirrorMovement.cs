using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    private Animator anim; //The animation of the enegy pulse
    private float lifeTime;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
        lifeTime = 5f;
    }

    void Update()
    {
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            anim.SetTrigger("End");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HealthComponent>() && !collision.CompareTag("Player"))
        {
            anim.SetTrigger("End");
        }
    }


}
