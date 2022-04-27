public class BonfireInteract : CampfireInteract
{
    protected override void Interact()
    {
        base.Interact();
        hp.HealDamage(int.MaxValue - hp.health, 0);
    }
}
