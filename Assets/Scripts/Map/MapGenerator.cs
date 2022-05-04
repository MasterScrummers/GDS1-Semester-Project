using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    private int[,] mapRoom;
    public GameObject startRoom;
    public GameObject[] rooms;
    public int roomCount;
    int maxRooms;

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
        mapRoom = new int[5+level,5+level];
        //-1 = starting room; 0:empty, 1:uend, 2:rend, 3:dend, 4:lend, 5:4way,
        //6:3wayu, 7:3wayr, 8:3wayd, 9:3wayl, 10:ur, 11:dr, 12:dl, 13ul, 
        Debug.Log(mapRoom.Length);
        int x = RandRoomCord(4 + level);
        int y = RandRoomCord(4 + level);
        mapRoom[x, y] = -1;
        roomCount = 1;
        maxRooms = 7 + level;

        while (roomCount < maxRooms)
        {
            mapRoom = RandomRoom(mapRoom, x, y, 4 + level);
        }


        InstantiateMap(5+level);

    }

    void InstantiateMap(int grid)
    {
        for(int i = 0; i < grid; i++)
        {
            for(int j = 0; j < grid; j++)
            {
                if (mapRoom[i, j] > 0)
                {
                    GameObject NewRoom = Instantiate(rooms[mapRoom[i,j]], new Vector3(i * 50, j * 50, 0), Quaternion.identity);
                }else if(mapRoom[i,j] == -1)
                {
                    GameObject start = Instantiate(startRoom, new Vector3(i * 50, j * 50, 0), Quaternion.identity);
                }
            }
        }
    }

    int RandRoomCord(int maxX)
    {
       return Random.Range(0, maxX);
    }

    int[,] RandomRoom(int[,] map, int x, int y, int max)
    {
        if (roomCount > maxRooms)
        {
            return map;
        }
        if (x <= max && y <= max && x >= 0 && y >= 0)
        {
            if (CheckRoomEmpty(map, x, y))
            {
                map[x, y] = 5;
                roomCount++;
            }
            else
            {
                int dir = Random.Range(1, 5);
                switch (dir)
                {
                    case 1:
                        map = RandomRoom(map, x, y + 1, max);
                        break;

                    case 2:
                        map = RandomRoom(map, x+1, y, max);
                        break;

                    case 3:
                        map = RandomRoom(map, x, y-1, max);
                        break;

                    case 4:
                        map = RandomRoom(map, x-1, y, max);
                        break;
                    default:
                        map = RandomRoom(map, x, y, max);
                        break;
                }
            }
        }
        return map;
    }

    bool CheckRoomEmpty(int[,] map, int x, int y)
    {
        return map[x, y] == 0;
    }
}
