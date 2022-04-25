using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonfireInteract : CampfireInteract
{
    // Start is called before the first frame update
    public override void Interact()
    {
        hp.HealDamage(0, 1);
        base.Interact();
    }
}
