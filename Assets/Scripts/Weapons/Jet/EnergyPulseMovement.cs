using UnityEngine;

public class EnergyPulseMovement : ProjectileMovement
{
    private Animator anim;
    EnergyPulseMovement() : base()
    {
        speed = 8f;
        lifeTime = 3f;
        
    }

    protected override void Start()
    {
        base.Start();
        anim = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.IsTouchingLayers(7))
        {
            anim.SetTrigger("End");
        }
    }
}
