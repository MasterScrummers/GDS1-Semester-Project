using UnityEngine;

public class NextLevel : MonoBehaviour
{
    //private bool playerContact;

    void Start()
    {
        //playerContact = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //playerContact = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //playerContact = false;
        }
    }
}
