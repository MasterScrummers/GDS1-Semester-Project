using UnityEngine;

public class HitBoxDetector : MonoBehaviour
{
    private bool isAttackOn = false; //Allow damage to apply.

    /// <summary>
    /// Should be called as an animation event.
    /// </summary>
    public void AttackStart()
    {
        isAttackOn = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isAttackOn)
        {
            return;
        }
        isAttackOn = false;
        Debug.Log("Collision");
    }
}
