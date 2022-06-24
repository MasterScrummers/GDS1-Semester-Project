#pragma warning disable IDE0051 // Remove unused private members
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponChest : InteractableObject
{
    private RoomData roomData; //The room data.
    private Animator anim; //WeaponChest animator
    private bool hasAppeared = false;
    private bool isOpened = false;

    protected override void Awake()
    {
        base.Awake();
        roomData = GetComponentInParent<RoomData>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        anim.SetBool("Appeared", hasAppeared);
        anim.SetBool("HasOpened", isOpened);
    }

    protected override void Interact()
    {
        base.Interact();
        ic.SetInputLock(true);
        anim.SetTrigger("Open");

    }

    private void OpenUI()
    {
        ic.SetInputLock(false);
        ic.GetComponent<UIController>().GetUI<UIWeaponSwapSystem>("WeaponSwapSystem").Activate(); //bring up WeaponSwap UI
    }

    private void SetHasAppeared()
    {
        hasAppeared = true;
    }

    private void SetHasOpened()
    {
        isOpened = true;
    }
}
