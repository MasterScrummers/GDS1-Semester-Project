using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutterMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    private Vector2 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        velocity = new Vector2(10f, 10f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
    }
}
