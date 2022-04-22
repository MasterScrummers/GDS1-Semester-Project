using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponChest : MonoBehaviour
{
    // Start is called before the first frame update
    private InputController ic; //Input controller to check inputs.
    private GameObject enemies; //Enemies parent object
    private SpriteRenderer chestSprite; //WeaponChest SpriteRenderer
    private Collider2D col; //WeaponChest collider

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        col = gameObject.GetComponent<Collider2D>();

        chestSprite = gameObject.GetComponent<SpriteRenderer>();
        chestSprite.enabled = false;

        enemies = GameObject.Find("Enemies");
    }

    // Update is called once per frame
    void Update()
    {
        //if all enemies in room are destroyed
        if (enemies && enemies.transform.childCount == 0)
        {
            chestSprite.enabled = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && chestSprite.enabled == true)
        {
            ic.GetComponent<UIController>().ActivateUI("WeaponSwapSystem", DoNothing); //bring up WeaponSwap UI
            col.enabled = false; //stops player from using chest again
        }
    }


    private void DoNothing() { }
}
