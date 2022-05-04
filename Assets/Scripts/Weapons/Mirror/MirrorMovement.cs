using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorMovement : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float radius = 1f, speed = 2f;
    float posX, posY, angle = 0f;
    
    // Update is called once per frame
    void Update()
    {
        posX = center.position.x + Mathf.Cos(angle) * radius;
        posY = center.position.y + Mathf.Sin(angle) * radius;
        transform.position = new Vector2(posX, posY);
        angle = angle + Time.deltaTime * speed;

        if (angle >= 360f)
        {
            angle = 0f;
        }
    }
}
