using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class WeaponCardGenerator : MonoBehaviour
{
    [System.Serializable]
    public class WeaponInfo
    {
        [SerializeField] private string weaponName;
        [SerializeField] private Sprite weaponSprite;

        public void AddIntoDictionary(ref Dictionary<string, Sprite> allWeapons)
        {
            allWeapons.Add(weaponName, weaponSprite);
        }
    }

    public WeaponBase weapon { get; private set; }
    [SerializeField] private WeaponInfo[] weapons;
    private Dictionary<string, Sprite> allWeapons;
    private Image weaponCard;
    [SerializeField] private Image weaponIcon;
    [SerializeField] private TextMeshProUGUI weaponText;

    void Start()
    {
        weaponCard = GetComponent<Image>();
        allWeapons = new Dictionary<string, Sprite>();
        foreach(WeaponInfo weapon in weapons)
        {
            weapon.AddIntoDictionary(ref allWeapons);
        }

        weapon = SetWeapon();
        SetColor();
        SetWeaponImage();
        SetWeaponText();
    }

    private WeaponBase SetWeapon()
    {
        return Random.Range(1, 4) switch
        {
            1 => new Sword(),
            //2 => new Hammer(),
            //3 => new Cutter(),
            _ => new Sword(),
        };
    }

    private void SetColor()
    {
        weaponCard.color = weapon.weaponColour; 
    }

    private void SetWeaponImage()
    {
        string weaponName = weapon.GetType().Name;
        weaponIcon.sprite = allWeapons[weaponName]; //The weapon name must be the same as the class name.
        switch(weaponName)
        {
            case "Sword":
                weaponIcon.rectTransform.eulerAngles = new Vector3(0, 0, 90);
                Vector2 size = weaponIcon.rectTransform.sizeDelta;
                float temp = size.x;
                size.x = size.y;
                size.y = temp;
                weaponIcon.rectTransform.sizeDelta = size;
                return;
        }
    }

    private void SetWeaponText()
    {
        weaponText.text = weapon.GetType().Name + "\nStrength: " + weapon.strength;
    }
}

