using UnityEngine;

public class Cutter : WeaponBase
{
    
    public Cutter() : base()
    {
        weaponName = "Cutter";
        description = "Cutter";
        animPath = basePath + "Cutter.";
    }

    public override void LightAttack(Animator anim)
    {
        anim.Play(animPath + "CutterLight");
    }
    // Update is called once per frame

}
