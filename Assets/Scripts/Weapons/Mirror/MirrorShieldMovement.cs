using UnityEngine;

public class MirrorShieldMovement : MonoBehaviour
{
    private Vector3 originalScale;
    public float scaleMultiplier; //The scale want to incrased to
    [HideInInspector] public bool IsSpecialAttack;
    private Animator anim;
    private VectorLerper lerp;
    private Timer lifeTime = new(0.5f);

    private void Awake()
    {
        originalScale = transform.localScale;
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (IsSpecialAttack)
        {
            SpecialAttack();
            return;
        }
        LightAttack();
    }

    private void OnEnable()
    {
        lifeTime.Reset();
        transform.localScale = originalScale;
        lerp = new VectorLerper(originalScale, originalScale * scaleMultiplier, 3f);
    }

    private void SpecialAttack()
    {
        lerp.Update(Time.deltaTime);
        transform.localScale = lerp.currentValue;
        if (transform.localScale.x >= scaleMultiplier)
        {
            anim.SetTrigger("End");
        }

    }

    private void LightAttack()
    {
        lifeTime.Update(Time.deltaTime);
        if (lifeTime.tick <= 0)
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
