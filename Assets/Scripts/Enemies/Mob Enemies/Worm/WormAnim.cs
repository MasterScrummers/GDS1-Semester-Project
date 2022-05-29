using UnityEngine;

public class WormAnim : MonoBehaviour
{
    private Worm worm; // Worm parent
    private PoolController pc;
    private Animator anim; // Worm sprite Animator
    [SerializeField] private Transform firePoint;
    private Worm.State state; // Tracks worm current state
    private Worm.State prevState;

    // Start is called before the first frame update
    void Start()
    {
        worm = GetComponentInParent<Worm>();
        anim = GetComponent<Animator>();
        pc = DoStatic.GetGameController<PoolController>();


        state = worm.state;
    }

    // Update is called once per frame
    void Update()
    {
        if (state != worm.state)
        {
            updateState();
        }
    }

    public void updateState() {
        prevState = state;
        state = worm.state;

        if (prevState == Worm.State.Hiding)
        {
            anim.Play("Base Layer.WormEmerge");
        }

        switch(state) {

            case Worm.State.Idle:
                anim.SetTrigger("Idle");
                break;
            case Worm.State.Attack:
                anim.SetTrigger("Attack");
                break;
            case Worm.State.Hiding:
                anim.SetTrigger("Burrow");
                break;
        }
    }

    public void Attack(float angle)
    {
        GameObject bullet = pc.GetObjectFromPool("WormBulletPool");
        bullet.transform.position = firePoint.position;
        Vector3 rot = bullet.transform.eulerAngles;
        rot.z = angle;
        bullet.transform.eulerAngles = rot;
    }

    public void Death()
    {
        worm.state = Worm.State.Death;
        anim.Play("Base Layer.WormDeath");
    }

    private void FinishDeath()
    {
        worm.RemoveEnemy();
    }
}
