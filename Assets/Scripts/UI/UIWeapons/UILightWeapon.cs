public class UILightWeapon : UIWeaponCircle
{
    void Update()
    {
        if (playerInput.lightWeapon != null)
        {
            UpdateColour(playerInput.lightWeapon);
        }
    }
}
