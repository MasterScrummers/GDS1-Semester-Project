using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextLevel : MonoBehaviour
{
    bool playerContact;

    void Start()
    {
        playerContact = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerContact = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        playerContact = false;
    }
}
