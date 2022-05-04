using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponChest : InteractableObject
{
    private RoomData roomData; //The room data.
    private SpriteRenderer chestSprite; //WeaponChest SpriteRenderer
    private Animator anim; //WeaponChest animator
    private bool hasAppeared = false;
    private bool isOpened = false;

    protected override void Awake()
    {
        base.Awake();
        chestSprite = GetComponent<SpriteRenderer>();
        chestSprite.enabled = false;

        roomData = GetComponentInParent<RoomData>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        bool set = roomData && roomData.empty;
        chestSprite.enabled = set;
        anim.SetBool("Visible", set);
        anim.SetBool("Appeared", hasAppeared);
        anim.SetBool("HasOpened", isOpened);
    }

    protected override void Interact()
    {
        base.Interact();
        anim.SetTrigger("Open");
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
