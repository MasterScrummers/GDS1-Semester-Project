using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRooms : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject gameController;
    public int doorPos;
    private GameObject kirby;
    void Start()
    {
        gameController = GameObject.FindWithTag("GameController");
        kirby = GameObject.FindWithTag("Player");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            MovePlayerRoom();
        }
    }

    private void MovePlayerRoom()
    {
        if(doorPos == 0)
        {
            kirby.transform.position += new Vector3(0f, 38f, 0f);
            gameController.transform.position += new Vector3(0f, 50f, 0f);
            
        }else if(doorPos == 1)
        {
            kirby.transform.position += new Vector3(35.5f, 0f, 0f);
            gameController.transform.position += new Vector3(50f, 0f, 0f);
        }
        else if(doorPos == 2)
        {
            kirby.transform.position += new Vector3(0f, -38f, 0f);
            gameController.transform.position += new Vector3(0f, -50f, 0f);
        }
        else
        {
            kirby.transform.position += new Vector3(-35.5f, 0f, 0f);
            gameController.transform.position += new Vector3(-50f, 0f, 0f);
        }
    }
}
