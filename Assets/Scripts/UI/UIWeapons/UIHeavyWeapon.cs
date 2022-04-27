public class UIHeavyWeapon : UIWeaponCircle
{
    void Update()
    {
        if (playerInput.heavyWeapon != null)
        {
            UpdateColour(playerInput.heavyWeapon);
            UpdateSprite(playerInput.heavyWeapon);
        }
    }
}
