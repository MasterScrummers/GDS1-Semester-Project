using UnityEngine;

public class MinotaurAnim : MonoBehaviour
{
    private MinotaurBoss boss;

    void Start()
    {
        boss = GetComponentInParent<MinotaurBoss>();
    }

    private void FinishDeath()
    {
        boss.FinishDeath();
    }
}
