using UnityEngine;

public abstract class UIWeaponCircle : UICircleBarBase
{
    protected PlayerInput playerInput;
    protected Color prevColor;

    protected override void Start()
    {
        base.Start();
        playerInput = DoStatic.GetPlayer().GetComponent<PlayerInput>();
    }

    protected void UpdateColour(WeaponBase weapon)
    {
        circle.color = weapon.weaponColour;
    }
}
