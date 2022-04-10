using UnityEngine;

public class UISpecialWeapon : UIWeaponCircle
{
    private Color coolingDown;

    protected override void Start()
    {
        base.Start();
        coolingDown = Color.gray;
    }

    protected override void Update()
    {
        if (assignedWeapon == null)
        {
            GetAssignedWeapon();
        }
        else
        {
            circle.color = circle.fillAmount == 1 ? assignedWeapon.weaponColour : coolingDown;
        }
        circle.fillAmount = playerInput.CooldownPercentage();
    }

    protected override void GetAssignedWeapon()
    {
        assignedWeapon = playerInput.specialWeapon;
    }
}
