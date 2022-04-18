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
    void Start()
    {
        allWeapons = new Dictionary<string, Sprite>();
        foreach (WeaponInfo weapon in weapons)
        {
            weapon.AddIntoDictionary(ref allWeapons);
        }
    }

    public Sprite GetWeapon(string name)
    {
        return allWeapons[name];
    }
}
