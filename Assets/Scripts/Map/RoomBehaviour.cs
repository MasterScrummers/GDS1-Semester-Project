using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomBehaviour : MonoBehaviour
{
    public int doorDirections;
    public GameObject gameController; //the GameController 

    void Start()
    {
        gameController = GameObject.Find("GameController");
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
