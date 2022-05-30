public class CampfireInteract : InteractableObject
{
    protected HealthComponent hp;
 
    protected override void Awake()
    {
        base.Awake();
        hp = DoStatic.GetPlayer<HealthComponent>();
    }

    protected override void Interact()
    {
        base.Interact();
        hp.OffsetHP(1);
    }
}
