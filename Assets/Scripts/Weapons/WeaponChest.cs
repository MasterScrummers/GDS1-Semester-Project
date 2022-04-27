using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class WeaponChest : InteractableObject
{
    private RoomData roomData; //The room data.
    private SpriteRenderer chestSprite; //WeaponChest SpriteRenderer
    private Animator anim; //WeaponChest animator

    protected override void Start()
    {
        base.Start();
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
    }

    protected override void Interact()
    {
        base.Interact();
        anim.SetBool("Open", true);
        ic.GetComponent<UIController>().ActivateUI("WeaponSwapSystem"); //bring up WeaponSwap UI
    }
}
