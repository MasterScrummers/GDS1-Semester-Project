using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CampfireInteract : InteractableObject
{
    public HealthComponent hp;
 
    private void Start()
    {
        hp = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthComponent>();
    }

    public override void Interact()
    {
        base.Interact();
        hp.HealDamage(1);
    }
}
