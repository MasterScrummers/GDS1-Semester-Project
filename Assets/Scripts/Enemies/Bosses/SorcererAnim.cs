using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SorcererAnim : MonoBehaviour
{
    private Sorcerer sorc; // Sorcerer parent
    private CapsuleCollider2D cc; // Sorcerer CapsuleCollider
    private PoolController pc;
    [SerializeField] private Transform firePoint;
    private Sorcerer.State state;
    private GameObject player;
    float circleAngle;
    float circleDirection = 1;

    // Start is called before the first frame update
    void Start()
    {
        cc = GetComponent<CapsuleCollider2D>();
        pc = DoStatic.GetGameController<PoolController>();
        sorc = GetComponentInParent<Sorcerer>();
        player = DoStatic.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {   

    }

    private void AttackLine(bool on)
    {
        if (on)
        {
            InvokeRepeating("AttackLine", 0f, 0.2f);
        } else {
            CancelInvoke();
        }
    }

    public void AttackLine()
    {
        GameObject bullet = pc.GetObjectFromPool("SorcBulletPool");
        bullet.transform.position = firePoint.position;
        DoStatic.LookAt(bullet.transform, player.transform);
    }

    private void AttackCircle(bool on) 
    {
        if (on)
        {
            InvokeRepeating("AttackCircle", 0f, 0.05f);
        } else {
            CancelInvoke();
        }
    }

    public void AttackCircle()
    {
        GameObject bullet = pc.GetObjectFromPool("SorcBulletPool");
        bullet.transform.position = firePoint.position;
        Vector3 rot = bullet.transform.eulerAngles;
        rot.z = circleAngle;
        bullet.transform.eulerAngles = rot;

        switch (circleAngle) {
            case 90:
                circleDirection = -1;
                break;
            case -90:
                circleDirection = 1;
                break;
        }

        circleAngle += circleDirection == 1 ? -10f : 10f;
    }

    public void UpdateState() {
        state = sorc.state;

        switch (state) {
            case Sorcerer.State.AttackLine:
                AttackLine(true);
                break;
            case Sorcerer.State.AttackCircle:
                AttackCircle(true);
                circleAngle = -90f;
                break;
            case Sorcerer.State.Move:
                AttackLine(false);
                AttackCircle(false);
                break;
        }
    }
}
