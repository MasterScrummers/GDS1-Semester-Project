using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiMovement : MonoBehaviour
{
    private Rigidbody2D rb; //The rigidbody of the gameobject
    [SerializeField] private Vector2 speed; //The speed of the energy pulse.
    private float lifeTime;
    private SpriteRenderer sr;
    void Start()
    {
        lifeTime = 3f;
        rb = GetComponent<Rigidbody2D>();
        //speed *= DoStatic.GetPlayer().transform.eulerAngles.y == 0 ? 1 : -1;
        sr = GetComponentInChildren<SpriteRenderer>();
        if (DoStatic.GetPlayer().transform.eulerAngles.y != 0)
        {
            sr.flipX = true;
        }
    }

    private void Update()
    {
        /*
        lifeTime -= Time.deltaTime;
        if (lifeTime <= 0)
        {
            Destroy(gameObject);
        }
        */
        float spd = 8f;
        rb.velocity = transform.right * (transform.eulerAngles.z / 360) * spd;
    }
}
