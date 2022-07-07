using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(SpriteFadeIn))]
public class SorcererProtectorBehaviour : AttackDealer
{
    private Collider2D col;
    private SpriteFadeIn spriteFadeIn;

    void Start()
    {
        col= GetComponent<Collider2D>();
        spriteFadeIn = GetComponent<SpriteFadeIn>();
    }

    protected override void Update()
    {
        base.Update();
        col.enabled = !spriteFadeIn.alpha.isLerping;
    }
}
