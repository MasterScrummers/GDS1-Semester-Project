using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int[,] mapRoom;
    private int[] direction; 

    void Start()
    {
        GenerateRooms(1);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void GenerateRooms(int level)
    {
        mapRoom = new int[3+level,3+level];
        //-1 = starting room; 0:empty, 1:uend, 2:rend, 3:dend, 4:lend, 5:4way,
        //6:3wayu, 7:3wayr, 8:3wayd, 9:3wayl, 10:ur, 11:dr, 12:dl, 13ul, 
        


        //InstantiateMap(3+level);

    }

    void InstantiateMap(int grid)
    {
        for(int i = 0; i < grid; i++)
        {
            for(int j = 0; j < grid; j++)
            {
                if (mapRoom[i, j] > 0)
                {
                    GameObject NewRoom = Instantiate(gameObject, new Vector3(i * 50, j * 50, 0), Quaternion.identity);
                }
            }
        }
    }
}
