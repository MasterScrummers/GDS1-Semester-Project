using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : InteractableObject
{
    // Start is called before the first frame update
    private InputController ic; //Input controller to check inputs.
    private GameObject enemies; //Enemies parent object
    private SpriteRenderer chestSprite; //WeaponChest SpriteRenderer
    private Collider2D col; //WeaponChest collider
    private Animator anim; //WeaponChest animator
    //private bool open; //Whether or not the chest needs to be opened

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        col = GetComponent<Collider2D>();

        chestSprite = GetComponent<SpriteRenderer>();
        chestSprite.enabled = false;

        enemies = GameObject.Find("/"+transform.parent.name+"/Enemies");

        anim = GetComponent<Animator>();

        //open = false;
    }

    // Update is called once per frame
    void Update()
    {
        //if all enemies in room are destroyed
        if (enemies && enemies.transform.childCount == 0)
        {
            chestSprite.enabled = true;
            anim.SetBool("Visible", true);
        }

        /*while (open == true)
        {
            if (Input.GetAxisRaw("Interact")>0)
            {
                OpenChest();
            }
        }*/

    }

    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && chestSprite.enabled == true)
        {
            open = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && chestSprite.enabled == true)
        {
            open = false;
        }
    }*/


    private void DoNothing() { }

    public override void Interact()
    {
        base.Interact();
        anim.SetBool("Open", true);
        ic.GetComponent<UIController>().ActivateUI("WeaponSwapSystem", DoNothing); //bring up WeaponSwap UI
        col.enabled = false; //stops player from using chest again
    }
}
