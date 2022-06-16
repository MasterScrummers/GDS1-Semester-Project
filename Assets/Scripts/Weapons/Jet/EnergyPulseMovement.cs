using UnityEngine;

public class EnergyPulseMovement : ProjectileMovement
{
    private Animator anim;

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
