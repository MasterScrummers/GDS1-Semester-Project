using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class WeaponCardGenerator : MonoBehaviour
{
    public PlayerWeaponBase weapon;
    private VariableController vc; //To grab the dictionary of weapons
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponText;

    void Start()
    {
        vc = DoStatic.GetGameController<VariableController>();
    }

    public void SetWeapon(PlayerWeaponBase weapon)
    {
        this.weapon = weapon;
        SetCard();
    }

    public void GenerateWeapon(OriginalValue<float> speed)
    {
        weapon = PlayerWeaponBase.RandomWeapon(speed);
        SetCard();
    }

    private void SetCard()
    {
        SetWeaponImage();
        SetWeaponText();
    }

    private void SetWeaponImage()
    {
        weaponIcon.sprite = vc.GetIcon(weapon.weaponName); //The weapon name must be the same as the class name.
        weaponIcon.SetNativeSize();
        switch(weapon.weaponName)
        {
            case "Sword":
            case "Cutter":
                weaponIcon.rectTransform.eulerAngles = new Vector3(0, 0, 90);
                weaponIcon.rectTransform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
                return;

            default:
                weaponIcon.rectTransform.eulerAngles = Vector3.zero;
                weaponIcon.rectTransform.localScale = new Vector3(0.65f, 0.65f, 0.65f); 
                return;
        }
    }

    private void SetWeaponText()
    {

        weaponText.text = "<b>" + weapon.GetType().Name + "</b>" + "\n\nStrength: " + weapon.strength +"\n\n" + weapon.description;
        weaponText.text += "\n(" + weapon.specialCooldown + " seconds cooldown)";
    }
}

