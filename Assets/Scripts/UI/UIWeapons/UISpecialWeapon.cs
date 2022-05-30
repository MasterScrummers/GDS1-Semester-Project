public class UISpecialWeapon : UIWeaponCircle
{
    protected void Update()
    {
        circle.fillAmount = playerInput.CooldownPercentage();
        circle.color = variableController.GetColour(circle.fillAmount == 1 ? "Rubik Green" : "Gray");
        UpdateSprite(playerInput.specialWeapon);
    }
}
