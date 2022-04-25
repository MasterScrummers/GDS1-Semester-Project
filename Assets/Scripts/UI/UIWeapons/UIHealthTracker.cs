using UnityEngine;

public class UIHealthTracker : UICircleBarBase
{
    private HealthComponent health;
    private bool isDead = false;

    protected override void Start()
    {
        base.Start();
        health = DoStatic.GetPlayer<HealthComponent>();
    }

    void Update()
    {
        if (!health)
        {
            return;
        }

        circle.fillAmount = health.GetPercentage();
        if (circle.fillAmount == 0 && !isDead)
        {
            health.GetComponentInChildren<PlayerAnim>().Death();
            isDead = true;
        }
    }
}
