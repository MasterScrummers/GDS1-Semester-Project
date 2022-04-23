using UnityEngine;

public class UISpecialWeapon : UIWeaponCircle
{
    private Color coolingDown;

    protected override void Start()
    {
        base.Start();
        coolingDown = Color.gray;
    }

    protected void Update()
    {
        circle.fillAmount = 1;
        if (playerInput.specialWeapon != null)
        {
            circle.color = circle.fillAmount == 1 ? playerInput.specialWeapon.weaponColour : coolingDown;
            circle.fillAmount = playerInput.CooldownPercentage();
            UpdateSprite(playerInput.specialWeapon);
        }
    }
}
