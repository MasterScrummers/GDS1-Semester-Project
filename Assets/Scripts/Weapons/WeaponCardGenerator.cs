using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class WeaponCardGenerator : MonoBehaviour
{
    public WeaponBase weapon;
    private VariableController vc; //To grab the dictionary of weapons
    private Image weaponCard;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponText;

    void Start()
    {
        vc = DoStatic.GetGameController<VariableController>();
        weaponCard = GetComponent<Image>();
    }

    public void SetWeapon(WeaponBase weapon)
    {
        this.weapon = weapon;
        SetCard();
    }

    public void GenerateWeapon()
    {
        weapon = RandomWeapon();
        SetCard();
    }

    private void SetCard()
    {
        SetColor();
        SetWeaponImage();
        SetWeaponText();
    }

    private WeaponBase RandomWeapon()
    {
        return Random.Range(1, 4) switch
        {
            1 => new Sword(),
            2 => new Hammer(),
            3 => new Cutter(),
            _ => new Sword(),
        };
    }

    private void SetColor()
    {
        weaponCard.color = weapon.weaponColour; 
    }

    private void SetWeaponImage()
    {
        weaponIcon.sprite = vc.GetWeapon(weapon.weaponName); //The weapon name must be the same as the class name.
        weaponIcon.SetNativeSize();
        switch(weapon.weaponName)
        {
            case "Sword":
            case "Cutter":
                weaponIcon.rectTransform.eulerAngles = new Vector3(0, 0, 90);
                weaponIcon.rectTransform.localScale = new Vector3(0.65f, 0.65f, 0.65f);
                return;

            case "Hammer":
                weaponIcon.rectTransform.eulerAngles = Vector3.zero;
                weaponIcon.rectTransform.localScale = new Vector3(0.65f, 0.65f, 0.65f); 
                return;
        }
    }

    private void SetWeaponText()
    {

        weaponText.text = "<b>" + weapon.GetType().Name + "</b>" + "\n\nStrength: " + weapon.baseStrength +"\n\n" + weapon.description;
        weaponText.text += "\n(" + weapon.specialCooldown + " seconds cooldown)";
    }
}

