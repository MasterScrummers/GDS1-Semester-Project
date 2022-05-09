using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //private int[,] mapRoom;
    [SerializeField] private int maxRooms; // The max amount of rooms
    public GameObject startRoom; //The start room, could be hardcoded.
    public RoomData[] specialRooms; //The special room pool
    public RoomData[] rooms; //The room pools
    //public int roomCount;

    private Dictionary<Vector2, RoomData> grid;
    private Dictionary<string, List<GameObject>> sortedNormalRooms;

    void Start()
    {
        grid = new Dictionary<Vector2, RoomData>();
        sortedNormalRooms = new Dictionary<string, List<GameObject>>();
        //GenerateRooms(1);

        GenerateLevel();
    }

    private void GenerateLevel()
    {
        SortRooms();
        PlanLevelStructure();
        GenerateRooms();
    }

    private void SortRooms()
    {
        foreach (string combination in new string[]
        {
            "L", "R", "U", "D",

            "LR", "LU", "LD",
            "RU", "RD",
            "UD",

            "LRU", "LRD", "LUD", "RUD",

            "LRUD"
        })
        {
            sortedNormalRooms.Add(combination, new List<GameObject>());
        }

        foreach (RoomData room in rooms)
        {
            string exits = "";
            exits += room.HasExit(RoomData.Direction.left) ? "L" : "";
            exits += room.HasExit(RoomData.Direction.right) ? "R" : "";
            exits += room.HasExit(RoomData.Direction.up) ? "U" : "";
            exits += room.HasExit(RoomData.Direction.down) ? "D" : "";
            sortedNormalRooms[exits].Add(room.gameObject);
        }
    }

    private void PlanLevelStructure()
    {
        void GenerateNeighbours(Queue<Vector2> unfilledRooms, Vector2 room, bool[] branches) {
            void AddEmptyRoom(Vector2 pos, bool allowBranch)
            {
                if (allowBranch && !grid.ContainsKey(pos))
                {
                    grid.Add(pos, null);
                    unfilledRooms.Enqueue(pos);
                    maxRooms--;
                }
            }
            
            AddEmptyRoom(room + new Vector2(-50, 0), branches[0]);
            AddEmptyRoom(room + new Vector2(50, 0), branches[1]);
            AddEmptyRoom(room + new Vector2(0, 50), branches[2]);
            AddEmptyRoom(room + new Vector2(0, -50), branches[3]);
        }

        bool[] RandomiseExits(float successRate, bool guarenteeAll)
        {
            bool[] branches = new bool[4];
            float chance = guarenteeAll ? 1 : successRate;
            for (int i = 0; i < branches.Length; i++)
            {
                branches[i] = DoStatic.RandomBool(chance);
            }

            return branches;
        }

        Queue<Vector2> unfilledRooms = new Queue<Vector2>();
        grid.Add(Vector2.zero, null);
        unfilledRooms.Enqueue(Vector2.zero);
        maxRooms--;

        while (maxRooms > 0)
        {
            GenerateNeighbours(unfilledRooms, unfilledRooms.Dequeue(), RandomiseExits(0.25f, unfilledRooms.Count == 0));
        }

    }

    private void GenerateRooms()
    {
        /// Work for Richard
        /// 
        /// First get list of all the predefined rooms from grid.
        /// Randomly select rooms and assign it to special rooms (Hard code the minotaur room for now)
        ///     - Keep in mind of the number of exits in the speical rooms
        /// 
        /// Loop through all rooms
        ///     if rooom has been assigned, skip iteration
        ///     else
        ///         consider the number of neighbours for the room and assign it to a random normal room accordingly
        ///             - Utilise the sorted room dictionary I made for you.
        ///    

        List<Vector2> spent = new List<Vector2>();
        Instantiate(specialRooms[2].gameObject, new Vector2(100, 0), Quaternion.identity);
        spent.Add(new Vector2(100, 0));
        Instantiate(specialRooms[0], new Vector2(150, 0), Quaternion.identity);
        spent.Add(new Vector2(150, 0));

        foreach (Vector2 pos in grid.Keys)
        {
            string exist = "";
            exist += grid.ContainsKey(pos + new Vector2(-50, 0)) ? "L" : "";
            exist += grid.ContainsKey(pos + new Vector2(50, 0)) ? "R" : "";
            exist += grid.ContainsKey(pos + new Vector2(0, 50)) ? "U" : "";
            exist += grid.ContainsKey(pos + new Vector2(0, -50)) ? "D" : "";

            if (!spent.Contains(pos))
            {
                List<GameObject> eligibleRoom = sortedNormalRooms[exist];
                int spawningRoom = Random.Range(0, eligibleRoom.Count);
                GameObject roomToSpawn = eligibleRoom[spawningRoom];

                Instantiate(roomToSpawn, pos, Quaternion.identity);
            }
        }
    }

    /*    void GenerateRooms(int level)
        {
            mapRoom = new int[5+level,5+level];
            //-1 = starting room; 0:empty, 1:uend, 2:rend, 3:dend, 4:lend, 5:4way,
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
                    map[x, y] = Random.Range(1,13);
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
        }*/
}
