public class UILightWeapon : UIWeaponCircle
{
    protected override void GetAssignedWeapon()
    {
        assignedWeapon = playerInput.heavyWeapon;
    }
}
