using UnityEngine;

public class MirrorAttackAnim : MonoBehaviour
{
    [SerializeField] GameObject mirror;
    [SerializeField] GameObject shield;

    private void ActiveMirror()
    {
        mirror.SetActive(true);
    }

    private void SetMirrorShield(string state)
    {
        shield.SetActive(state.Equals("On"));
    }
}
