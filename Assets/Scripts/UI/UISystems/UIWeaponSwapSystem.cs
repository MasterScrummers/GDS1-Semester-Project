using UnityEngine;

public class UIWeaponSwapSystem : UISystemBase
{
    private PlayerInput input; //The player input.
    [SerializeField] private WeaponCardGenerator lightCard; //The light card
    [SerializeField] private WeaponCardGenerator heavyCard; //The heavy card
    [SerializeField] private WeaponCardGenerator specialCard; //The special card
    [SerializeField] private WeaponCardGenerator newCard; //The new card.

    public override void Initiate()
    {
        base.Initiate();
        input = DoStatic.GetPlayer<PlayerInput>();
    }
    protected override void FirstActiveFrameUpdate()
    {
        base.FirstActiveFrameUpdate();
        UpdateCards();
        newCard.GenerateWeapon(input.speed);
    }

    void Update()
    {
        void WeaponSwap(ref PlayerWeaponBase weapon)
        {
            PlayerWeaponBase temp = newCard.weapon;
            newCard.SetWeapon(weapon);
            weapon = temp;
            UpdateCards();
        }

        if (base.DoTransitioning())
        {
            return;
        }

        ic.SetInputReason("Attack", false);
        input.allowMovement = false;

        if (ic.GetButtonDown("WeaponSwap", "Exit"))
        {
            Deactivate();
            return;
        }

        if (ic.GetButtonDown("WeaponSwap", "Light") && input.lightWeapon != null)
        {
            WeaponSwap(ref input.lightWeapon);
        }
        
        if (ic.GetButtonDown("WeaponSwap", "Heavy") && input.heavyWeapon != null)
        {
            WeaponSwap(ref input.heavyWeapon);
        }
        
        if (ic.GetButtonDown("WeaponSwap", "Special") && input.specialWeapon != null)
        {
            WeaponSwap(ref input.specialWeapon);
        }
    }

    private void UpdateCards()
    {
        if (input.lightWeapon != null)
        {
            lightCard.SetWeapon(input.lightWeapon);
        }

        if (input.heavyWeapon != null)
        {
            heavyCard.SetWeapon(input.heavyWeapon);
        }

        if (input.specialWeapon != null)
        {
            specialCard.SetWeapon(input.specialWeapon);
        }
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        ic.SetInputReason("Attack", true);
        input.allowMovement = true;
    }
}
