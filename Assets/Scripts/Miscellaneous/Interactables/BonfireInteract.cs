public class BonfireInteract : CampfireInteract
{
    protected override void Interact()
    {
        base.Interact();
        hp.HealDamage(0, 1);
    }
}
