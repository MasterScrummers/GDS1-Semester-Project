#pragma warning disable IDE0051
using UnityEngine;

public class KirbyDeathEffect : MonoBehaviour
{
    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponentInParent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void DeathJump()
    {
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        rb.AddForce(transform.up * 20f, ForceMode2D.Impulse);
    }

    private void DeathRotate()
    {
        Vector3 rot = rb.transform.eulerAngles;
        rot.z -= 90;
        rb.transform.eulerAngles = rot;
    }

    private void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
