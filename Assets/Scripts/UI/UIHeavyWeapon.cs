public class UIHeavyWeapon : UIWeaponCircle
{
    protected override void GetAssignedWeapon()
    {
        assignedWeapon = playerInput.lightWeapon;
    }
}
