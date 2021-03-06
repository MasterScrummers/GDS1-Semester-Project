using UnityEngine;
using UnityEngine.UI;

public abstract class UIWeaponCircle : UICircleBarBase
{
    protected PlayerInput playerInput;
    protected Image weaponIcon;
    protected VariableController variableController;

    protected override void Start()
    {
        base.Start();
        foreach(Transform child in DoStatic.GetChildren(transform))
        {
            weaponIcon = child.GetComponent<Image>();
            if (weaponIcon)
            {
                break;
            }
        }
        variableController = DoStatic.GetGameController<VariableController>();
        playerInput = DoStatic.GetPlayer<PlayerInput>();
        circle.color = variableController.GetColour("Rubik Green");
    }

    protected void UpdateSprite(PlayerWeaponBase weapon)
    {
        weaponIcon.sprite = variableController.GetIcon(weapon.weaponName);
        weaponIcon.SetNativeSize();
        switch (weapon.weaponName)
        {
            case "Sword":
            case "Cutter":
                weaponIcon.rectTransform.eulerAngles = new Vector3(0, 0, 90);
                return;

            default:
                weaponIcon.rectTransform.eulerAngles = Vector3.zero;
                return;
        }
    }
}
