using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    private Animator anim; //The animation of the enegy pulse
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<HealthComponent>() && !collision.CompareTag("Player"))
        {
            anim.SetTrigger("End");
        }
    }


}
