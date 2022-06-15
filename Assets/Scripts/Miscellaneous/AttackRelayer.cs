using UnityEngine;

public class AttackRelayer : MonoBehaviour, IAttackReceiver
{
    [SerializeField] private GameObject receiver;
    private IAttackReceiver attackReceiver;

    private void Start()
    {
        attackReceiver = receiver.GetComponent<IAttackReceiver>();
    }

    public void RecieveAttack(Transform attackerPos, int strength, Vector2 knockback, float stunTime, bool calcFromAttackerPos = false)
    {
        attackReceiver?.RecieveAttack(attackerPos, strength, knockback, stunTime, calcFromAttackerPos);
    }
}
