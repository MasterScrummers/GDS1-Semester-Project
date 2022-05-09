using UnityEngine;

public class MirrorShieldMovement : MonoBehaviour
{
    [SerializeField] Vector3 originalScale;
    private float sizeCounter = 3f; //The time Shield stays in the original form
    private float lifeTime = 3; //Start afte sizeCounter ended, used to reduce the size of shield.
    [SerializeField] Animator playerAnim;


    // Start is called before the first frame update
    void Start()
    {
        OnEnable();
    }

    // Update is called once per frame
    void Update()
    {
        sizeCounter -= Time.deltaTime;
        if (sizeCounter <= 0)
        {
            lifeTime -= Time.deltaTime;

            if (lifeTime <= 2 && lifeTime > 0)
            {
                transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); 
            }

            if (lifeTime <= 0)
            {
                playerAnim.SetTrigger("ShieldBadEnd");
            }
        }
    }

    void OnEnable()
    {
        transform.localScale = originalScale;
        sizeCounter = 4f;
        lifeTime = 4;
    }
}
