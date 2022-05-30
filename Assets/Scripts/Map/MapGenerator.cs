using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    //private int[,] mapRoom;
    private VariableController vCont;
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
        vCont = DoStatic.GetGameController<VariableController>();
        SortRooms();
    }

    /// <summary>
    /// To be called ONCE per level by external means (usually by SceneStartUp).
    /// </summary>
    public void GenerateLevel()
    {
        PlanLevelStructure();
        GenerateRooms();
        grid.Clear(); //For the next time the level needs to be generated.
    }

    private void SortRooms()
    {
        foreach (string combination in new string[]
        {
            "L", "R", "U", "D",

            "LR", "LU", "LD",
            "UR", "DR",
            "UD",

            "LUR", "LDR", "LUD", "UDR",

            "LUDR"
        })
        {
            sortedNormalRooms.Add(combination, new List<GameObject>());
        }

        foreach (RoomData room in rooms)
        {
            string exits = "";
            exits += room.HasExit(RoomData.Direction.left) ? "L" : "";
            exits += room.HasExit(RoomData.Direction.up) ? "U" : "";
            exits += room.HasExit(RoomData.Direction.down) ? "D" : "";
            exits += room.HasExit(RoomData.Direction.right) ? "R" : "";
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
        int level = vCont.GetLevel();
        Vector2 maxRight = Vector2.zero;
        string finalCode = ""; 
        GameObject finalRoom = null;

        string[] cell = new string[sortedNormalRooms.Count];
        sortedNormalRooms.Keys.CopyTo(cell, 0);

        Vector2[] rooms = new Vector2[grid.Count];
        grid.Keys.CopyTo(rooms, 0);

        foreach (Vector2 pos in rooms)
        {
            string exist = "";
            exist += grid.ContainsKey((pos + new Vector2(-50, 0))) ? "L" : "";
            exist += grid.ContainsKey(pos + new Vector2(0, 50)) ? "U" : "";
            exist += grid.ContainsKey(pos + new Vector2(0, -50)) ? "D" : "";
            exist += grid.ContainsKey(pos + new Vector2(50, 0)) ? "R" : "";

            if (!grid[pos])
            {
                List<GameObject> eligibleRoom = sortedNormalRooms[exist];
                int spawningRoom = Random.Range(0, eligibleRoom.Count);
                GameObject roomToSpawn = eligibleRoom[spawningRoom];

                GameObject spawned = Instantiate(roomToSpawn, pos, Quaternion.identity);
                grid[pos] = spawned.GetComponent<RoomData>();

                if (pos.x > maxRight.x)
                {
                    maxRight = pos;
                    finalRoom = spawned;
                    finalCode = exist;
                }
            }
        }

        Destroy(finalRoom);
        finalCode += "R";
        List<GameObject> roomsT = sortedNormalRooms[finalCode];
        int randR = Random.Range(0, roomsT.Count);
        GameObject room = roomsT[randR];
        Instantiate(room, maxRight, Quaternion.identity);
        Instantiate(specialRooms[1], maxRight + new Vector2(50,0), Quaternion.identity);
        Instantiate(specialRooms[level + 1], maxRight + new Vector2(100, 0), Quaternion.identity);
        Instantiate(specialRooms[0], maxRight + new Vector2(150,0),Quaternion.identity);
    } 
}
