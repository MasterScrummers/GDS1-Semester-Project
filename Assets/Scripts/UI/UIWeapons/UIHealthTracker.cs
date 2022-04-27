using UnityEngine;

public class UIHealthTracker : UICircleBarBase
{
    private HealthComponent health;
    private bool isDead = false;

    protected override void Start()
    {
        base.Start();
        health = DoStatic.GetPlayer().GetComponent<HealthComponent>();
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
            DoStatic.GetPlayer().GetComponentInChildren<PlayerAnim>().Death();
            isDead = true;
        }
    }
}
