using UnityEngine;

public class MirrorShieldMovement : MonoBehaviour
{
    private Vector3 originalScale;
    public Vector3 scaleIncraseOverTime; //Amount of Scale Increase Over Time
    public float scaleIncrasedTo; //The scale want to incrased to
    [HideInInspector] public bool IsSpecialAttack;
    private Animator anim;
    private VectorLerper lerp;
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
        lerp.Update(Time.deltaTime);

    }

    private void OnEnable()
    {
        lifeTime = originalLifeTime;
        transform.localScale = originalScale;
        lerp = new VectorLerper(new Vector3(1.2f, 1.2f, 1.2f), new Vector3(5.2f, 5.2f, 5.2f), 3f);
    }

    private void SpecialAttack()
    {
        transform.localScale = lerp.currentValue;
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
