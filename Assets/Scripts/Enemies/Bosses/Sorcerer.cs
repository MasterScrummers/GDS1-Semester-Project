#pragma warning disable IDE0051 // Remove unused private members
#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class Sorcerer : Enemy
{
    public enum State { Idle, Busy };
    [field: Header("Sorcerer Parameters"), SerializeField] public State state { get; private set; } = State.Idle;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float minIdleTime = 1f;
    [SerializeField] private float maxIdleTime = 5f;
    [SerializeField] private GameObject protection;

    private HealthComponent hp;
    private Transform player;
    private Timer aiTimer;
    private PoolController pc;

    public enum ShootingStyles
    {
        Towards,
        Circle,
        Shotgun,
        Random,
    }
    private ShootingStyles shootingStyle = 0;

    private readonly OriginalValue<int> numberOfBullets = new(20);

    protected override void Start()
    {
        base.Start();
        hp = GetComponent<HealthComponent>();
        player = DoStatic.GetPlayer<Transform>();
        aiTimer = new(minIdleTime);
        pc = DoStatic.GetGameController<PoolController>();
        numberOfBullets.Reset();

        foreach (Transform child in DoStatic.GetChildren(protection.transform))
        {
            child.GetComponent<AttackDealer>().SetWeapon(weapon);
        }
    }

    protected override void DoAction()
    {
        aiTimer.Update(Time.deltaTime);
        if (state == State.Idle)
        {
            LookAtPlayer();
        }

        if (state != State.Idle || aiTimer.tick > 0)
        {
            return;
        }

        state = State.Busy;
        anim.SetTrigger("TimeToAttack");
        int typeOfAttack = Random.Range(0, 2);
        anim.SetInteger("TypeOfAttack", typeOfAttack);
        if (typeOfAttack == 0)
        {
            numberOfBullets.value = Random.Range((int)(numberOfBullets.originalValue * 0.5f), numberOfBullets.originalValue);
            shootingStyle = (ShootingStyles)Random.Range(0, System.Enum.GetNames(typeof(ShootingStyles)).Length);
        }
    }

    private void LookAtPlayer()
    {
        Vector3 sca = transform.localScale;
        sca.x = Mathf.Abs(sca.x);
        sca.x = transform.position.x < player.position.x ? sca.x : -sca.x;
        transform.localScale = sca;
    }

    protected override void Death()
    {
        anim.Play("Death");
    }

    private Transform ShootProjectile(float angle)
    {
        Transform bullet = pc.GetObjectFromPool("SorcBulletPool").transform;
        bullet.position = firePoint.position;
        Vector3 rot = bullet.eulerAngles;
        rot.z = angle;
        bullet.eulerAngles = rot;
        bullet.GetComponent<AttackDealer>().SetWeapon(weapon);
        return bullet;
    }

    #region Animation Events
    private void TeleportToPlayer()
    {
        float xOffset = 3f;
        transform.position = (Vector2)player.position + new Vector2(DoStatic.RandomBool() ? xOffset : -xOffset, 0);
        LookAtPlayer();
    }

    private void TeleportRandom()
    {
        transform.localPosition = new Vector2(Random.Range(-6.5f, 6.5f), Random.Range(-5.5f, 5.5f));
    }

    private void LoopToIdle()
    {
        state = State.Idle;
        aiTimer.SetTimer(minIdleTime + Random.Range(0 , maxIdleTime * hp.GetPercentage()));
    }

    private void Shoot()
    {
        if (numberOfBullets.value-- == 0)
        {
            numberOfBullets.Reset();
            anim.SetTrigger("LoopToIdle");
            LoopToIdle();
            return;
        }

        switch (shootingStyle)
        {
            case ShootingStyles.Towards:
                DoStatic.LookAt(ShootProjectile(0), player.position);
                break;

            case ShootingStyles.Circle:
                int bulletsInCircle = 5;
                float incrementalAngle = 360 / bulletsInCircle;
                float offsetAngle = Random.Range(-incrementalAngle, incrementalAngle);
                for (int i = 0; i < bulletsInCircle; i++)
                {
                    ShootProjectile(i * incrementalAngle + offsetAngle);
                }
                break;

            case ShootingStyles.Shotgun:
                int shotgunBullets = 3;
                for (int i = 0; i < shotgunBullets; i++)
                {
                    Transform bullet = ShootProjectile(0);
                    DoStatic.LookAt(bullet, player.position);
                
                    Vector3 bulletRot = bullet.eulerAngles;
                    bulletRot.z += Random.Range(-45, 45f);
                    bullet.eulerAngles = bulletRot;
                }
                break;

            case ShootingStyles.Random:
                int randomBullets = 10;
                for (int i = 0; i < randomBullets; i++)
                {
                    ShootProjectile(Random.Range(0, 360f));
                }
                break;
        }
    }
    #endregion
}
