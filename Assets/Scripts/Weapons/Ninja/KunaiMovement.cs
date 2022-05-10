using UnityEngine;

public class KunaiMovement : ProjectileBase
{
    public KunaiMovement()
    {
        speed = 8f;
        lifeTime = 3f;
    }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        base.Update();
        lifeTime -= Time.deltaTime;
        DestroySelf(lifeTime);
    }

}
