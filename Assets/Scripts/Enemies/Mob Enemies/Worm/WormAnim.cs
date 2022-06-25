using UnityEngine;

public class WormAnim : MonoBehaviour
{
    private PoolController pc;
    private AttackDealer attacker;
    private Animator anim; // Worm sprite Animator
    [SerializeField] private Transform firePoint;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        attacker = GetComponent<AttackDealer>();
        pc = DoStatic.GetGameController<PoolController>();
    }

    public void UpdateState(Worm.State state) {
        anim.SetBool("Idling", state == Worm.State.Idle);
        anim.SetBool("Attacking", state == Worm.State.Attack);
        anim.SetBool("Hiding", state == Worm.State.Hiding);
    }

    public void Attack()
    {
        GameObject bullet = pc.GetObjectFromPool("WormBulletPool");
        bullet.transform.position = firePoint.position;

        Vector3 rot = bullet.transform.eulerAngles;
        rot.z = Random.Range(45, 135) + transform.eulerAngles.z;
        bullet.transform.eulerAngles = rot;
        bullet.GetComponent<AttackDealer>().SetWeapon(attacker.weapon);
    }

    public void Death()
    {
        anim.Play("Base Layer.WormDeath");
    }
}
