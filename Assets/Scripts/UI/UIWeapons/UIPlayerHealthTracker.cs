public class UIPlayerHealthTracker : UICircleBarBase
{
    private HealthComponent hp; //To track the HP
    private VariableController vc;

    protected override void Start()
    {
        base.Start();
        hp = DoStatic.GetPlayer<HealthComponent>();
        vc = DoStatic.GetGameController<VariableController>();
    }

    void Update()
    {
        if (!hp)
        {
            return;
        }

        float percent = hp.GetPercentage();
        circle.fillAmount = percent;
        circle.color = vc.GetColour(percent > 0.25 ? "Rubik Green" : "Rubik Red");
    }
}
