public class UIHealthTracker : UICircleBarBase
{
    private HealthComponent hp; //To track the HP

    protected override void Start()
    {
        base.Start();
        hp = DoStatic.GetPlayer<HealthComponent>();
    }

    void Update()
    {
        if (!hp)
        {
            return;
        }

        circle.fillAmount = hp.GetPercentage();
    }
}
