using UnityEngine;

public class MirrorShieldMovement : MonoBehaviour
{
    private Vector3 originalScale;
    public Vector3 scaleIncraseOverTime; //Amount of Scale Increase Over Time
    public float scaleIncrasedTo; //The scale want to incrased to
    [HideInInspector] public bool IsSpecialAttack;
    private Animator anim;

    private float originalLifeTime;
    private float lifeTime;

    private void Awake()
    {
        originalLifeTime = 0.5f;
        originalScale = transform.localScale;
        anim = this.GetComponent<Animator>();

    }

    private void Update()
    {
        if (IsSpecialAttack)
        {
            SpecialAttack();
        }

        else if (!IsSpecialAttack)
        {
            LightAttack();
        }

    }

    private void OnEnable()
    {
        lifeTime = originalLifeTime;
        transform.localScale = originalScale;

    }

    private void SpecialAttack()
    {
        transform.localScale += scaleIncraseOverTime * Time.deltaTime;
        if (transform.localScale.x >= scaleIncrasedTo)
        {
            anim.SetTrigger("End");
        }

    }

    private void LightAttack()
    {
        lifeTime -= Time.deltaTime;
        //transform.localScale += scaleIncraseOverTime * Time.deltaTime;
        if (lifeTime <= 0)
        {
            DisableSelf();
        }

    }

    //Animation Related
    private void DisableSelf()
    {
        gameObject.SetActive(false);
    }

}
