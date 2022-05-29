public class BonfireInteract : CampfireInteract
{
    protected override void Interact()
    {
        base.Interact();
        hp.SetHP();
    }
}
