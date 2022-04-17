using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponCardGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public WeaponBase weapon;
    public Sprite sword;
    Image weaponCard;



    void Start()
    {
        
        weapon = SetWeapon();
        Debug.Log(weapon);
        weaponCard = GetComponent<Image>();
        SetColor();
        SetWeaponImage();
        SetWeaponText();
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private WeaponBase SetWeapon()
    {
        int weaponNum = Random.Range(1, 4);

        switch (weaponNum)
        {
            case 1:
                return new Sword();

            case 2:
                return new Hammer();

            case 3:
                return new Cutter();

            default:
                return new Sword();
        }
    }

    private void SetColor()
    {
        weaponCard.color = weapon.weaponColour; 
    }

    private void SetWeaponImage()
    {
        GameObject weaponIcon = new GameObject("WeaponIcon");
        weaponIcon.transform.SetParent(weaponCard.transform);
        weaponIcon.transform.localPosition = new Vector3(0f, 45f, 0f);
        weaponIcon.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        Image weaponImage = weaponIcon.AddComponent<Image>();


        if (weapon.GetType().Equals(typeof(Sword))){
            weaponImage.sprite = sword;
        }

        else if (weapon.GetType().Equals(typeof(Hammer)))
        {
            
        }

        else if (weapon.GetType().Equals(typeof(Cutter)))
        {

        }
    }

    private void SetWeaponText()
    {
        GameObject weaponInfo = new GameObject("WeaponInfo"); //New gameobject to store weapon text
        weaponInfo.transform.SetParent(weaponCard.transform); //Make weapon card the parent
        weaponInfo.transform.localPosition = new Vector3(0f, 0f, 0f); //Position relative to weapon card
        weaponInfo.transform.localScale = new Vector3(3f, 3f, 3f); //Scale relative to weapon card

        TextMeshPro weaponText = weaponInfo.AddComponent<TextMeshPro>(); //Add TextMeshPro component to empty gameobject
        weaponText.sortingLayerID = SortingLayer.NameToID("UI");
        weaponText.sortingOrder = 1; //Have text in front of weapon card
        weaponText.alignment = TextAlignmentOptions.Center; //Align text center

        if (weapon.GetType().Equals(typeof(Sword)))
        {
            weaponText.text = "Sword\n" +
                "Strength: " + weapon.strength;
        }

        else if (weapon.GetType().Equals(typeof(Hammer)))
        {
            weaponText.text = "Hammer\n" +
                "Strength: " + weapon.strength;
        }

        else if (weapon.GetType().Equals(typeof(Cutter)))
        {
            weaponText.text = "Cutter\n" +
                "Strength: " + weapon.strength;
        }
    }
}

