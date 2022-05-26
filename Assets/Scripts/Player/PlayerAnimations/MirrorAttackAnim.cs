using UnityEngine;

public class MirrorAttackAnim : MonoBehaviour
{
    [SerializeField] GameObject mirror;
    [SerializeField] GameObject shield;
    private MirrorShieldMovement ms;

    private void Start()
    {
        ms = shield.GetComponent<MirrorShieldMovement>();
    }
    private void ActiveMirror()
    {
        mirror.SetActive(true);
    }

    private void SetMirrorShield(string state)
    {
        shield.SetActive(state.Equals("On"));
    }

    private void SetIsMirrorSpecial(string state)
    {
        ms.IsSpecialAttack = state.Equals("True");
    }
}
