public class CampfireInteract : InteractableObject
{
    protected HealthComponent hp;
 
    protected override void Start()
    {
        base.Start();
        hp = DoStatic.GetPlayer<HealthComponent>();
    }

    protected override void Interact()
    {
        base.Interact();
        hp.HealDamage(1,0);
    }
}
