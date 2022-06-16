using UnityEngine;

public class AttackRelayer : MonoBehaviour, IAttackReceiver
{
    [SerializeField] private GameObject receiver;
    private IAttackReceiver attackReceiver;

    private void Start()
    {
        attackReceiver = receiver.GetComponent<IAttackReceiver>();
    }

    public void RecieveAttack(Transform attackerPos, WeaponBase weapon)
    {
        attackReceiver?.RecieveAttack(attackerPos, weapon);
    }
}
