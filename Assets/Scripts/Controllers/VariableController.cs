using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    [System.Serializable]
    private class WeaponInfo
    {
        [SerializeField] private string weaponName;
        [SerializeField] private Sprite weaponSprite;

        public void AddIntoDictionary(ref Dictionary<string, Sprite> allWeapons)
        {
            allWeapons.Add(weaponName, weaponSprite);
        }
    }

    [SerializeField] private WeaponInfo[] weapons;
    private Dictionary<string, Sprite> allWeapons;
    private Dictionary<WeaponBase.Affinity, Color32> globalColours;

    void Awake()
    {
        allWeapons = new();
        foreach (WeaponInfo weapon in weapons)
        {
            weapon.AddIntoDictionary(ref allWeapons);
        }

        globalColours = new()
        {
            [WeaponBase.Affinity.fire] = new(183, 18, 52, 255),
            [WeaponBase.Affinity.water] = new(0, 70, 173, 255),
            [WeaponBase.Affinity.grass] = new(0, 155, 72, 255),
        };
    }

    public Sprite GetWeapon(string name)
    {
        return allWeapons[name];
    }

    public Color32 GetColor(WeaponBase.Affinity affinity)
    {
        return globalColours[affinity];
    }
}
