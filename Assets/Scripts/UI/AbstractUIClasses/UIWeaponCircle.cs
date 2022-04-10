using UnityEngine;

public abstract class UIWeaponCircle : UICircleBarBase
{
    protected PlayerInput playerInput;
    protected Color prevColor;
    protected WeaponBase assignedWeapon;

    protected override void Start()
    {
        base.Start();
        playerInput = DoStatic.GetPlayer().GetComponent<PlayerInput>();
    }

    protected virtual void Update()
    {
        if (assignedWeapon == null)
        {
            GetAssignedWeapon();
        }
        else
        {
            circle.color = assignedWeapon.weaponColour;
        }
    }

    /// <summary>
    /// Meant to be overridden.
    /// </summary>
    protected virtual void GetAssignedWeapon() {}
}
