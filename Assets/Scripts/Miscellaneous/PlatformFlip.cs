using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformFlip : MonoBehaviour
{
    // Start is called before the first frame update
    private PlatformEffector2D effector;
    public float waitTime;
    private bool isTouching;
    void Start()
    {
        effector = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
            if(waitTime < 0)
        {
            effector.rotationalOffset = 0f;
        }

            if (Input.GetKeyUp(KeyCode.S))
            {
            waitTime = 0.3f;
            }

            if (Input.GetKey(KeyCode.S))
            {
                if (waitTime <= 0)
                {
                    effector.rotationalOffset = 180f;
                    waitTime = 0.3f;
                }

            }

        
            waitTime -= Time.deltaTime;
        
    }
}
