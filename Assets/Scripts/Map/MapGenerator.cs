using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [System.Serializable]
    private class SpecialRooms
    {
        public string roomName;
        public RoomData room;
    }

    private VariableController vCont; //To keep track of the levels.
    [SerializeField] private int maxRooms; // The max amount of rooms
    [SerializeField] private SpecialRooms[] specialRooms; //The special room pool
    [SerializeField] private RoomData[] rooms; //The room pools

    //public int roomCount;

    private Dictionary<Vector2, RoomData> grid = new();
    private Dictionary<string, List<GameObject>> sortedNormalRooms = new();

    private void Start()
    {
        vCont = DoStatic.GetGameController<VariableController>();
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
            string exits = room.hasLeft ? "L" : "";
            exits += room.hasRight ? "R" : "";
            exits += room.hasUp ? "U" : "";
            exits += room.hasDown ? "D" : "";
            sortedNormalRooms[exits].Add(room.gameObject);
        }
    }

    /// <summary>
    /// To be called ONCE per level by external means (usually by SceneStartUp).
    /// </summary>
    public void GenerateLevel()
    {
        PlotRooms(maxRooms, new(0, 0));
        RemoveSomeRooms();
        CheckAndRemoveDisconnectedRooms(new Vector2(0, 0));
        GenerateRooms();
    }

    private void PlotRooms(int roomCount, Vector2 startPos)
    {
        Vector2[] neighbours = new Vector2[] {
            startPos,
            startPos + new Vector2(50, 0),//1
            startPos + new Vector2(-50, 0),//2
            startPos + new Vector2(0, 50),//3
            startPos + new Vector2(0, -50),//4
        };

        foreach (Vector2 pos in neighbours)
        {
            if ((DoStatic.RandomBool() || pos == startPos) && !grid.ContainsKey(pos))
            {
                grid.Add(pos, null);
                roomCount--;
            }

            if (roomCount == 0)
            {
                return;
            }
        }

        PlotRooms(roomCount, neighbours[Random.Range(0, neighbours.Length)]);
    }

    private void RemoveSomeRooms()
    {
        Vector2[] locations = new Vector2[grid.Count];
        grid.Keys.CopyTo(locations, 0);
        foreach (Vector2 location in locations)
        {
            if (DoStatic.RandomBool() || !grid.ContainsKey(location) || location == new Vector2(0, 0))
            {
                continue;
            }

            bool safeToRemove = grid.ContainsKey(location + new Vector2(50, 0)); //Right
            safeToRemove = safeToRemove && grid.ContainsKey(location + new Vector2(-50, 0)); //Left
            safeToRemove = safeToRemove && grid.ContainsKey(location + new Vector2(0, 50)); //Top
            safeToRemove = safeToRemove && grid.ContainsKey(location + new Vector2(0, -50)); //Bottom

            if (!safeToRemove)
            {
                continue;
            }

            bool TRBL = grid.ContainsKey(location + new Vector2(50, 50));
            TRBL = TRBL && grid.ContainsKey(location + new Vector2(-50, -50)); //BottomLeft

            bool TLBR = grid.ContainsKey(location + new Vector2(-50, 50)); //TopLeft
            TLBR = TLBR && grid.ContainsKey(location + new Vector2(50, -50)); //BottomRight
            
            if (TLBR || TRBL)
            {
                grid.Remove(location);
            }
        }
    }

    private void CheckAndRemoveDisconnectedRooms(Vector2 starPos)
    {
        List<Vector2> history = new();
        Queue<Vector2> placesToCheck = new();
        placesToCheck.Enqueue(starPos);

        while (placesToCheck.Count > 0)
        {
            Vector2 position = placesToCheck.Dequeue();
            history.Add(position);

            foreach (Vector2 neighbour in new Vector2[] {
                position + new Vector2(50, 0),//1
                position + new Vector2(-50, 0),//2
                position + new Vector2(0, 50),//3
                position + new Vector2(0, -50),//4
            })
            {
                if (grid.ContainsKey(neighbour) && !history.Contains(neighbour) && !placesToCheck.Contains(neighbour))
                {
                    placesToCheck.Enqueue(neighbour);
                }
            }
        }
        
        Vector2[] locations = new Vector2[grid.Count];
        grid.Keys.CopyTo(locations, 0);
        foreach (Vector2 location in locations)
        {
            if (!history.Contains(location))
            {
                grid.Remove(location);
            }
        }
    }

    private void GenerateRooms()
    {
        Vector2[] locations = new Vector2[grid.Count];
        grid.Keys.CopyTo(locations, 0);
        foreach (Vector2 location in locations)
        {
            Instantiate(rooms[Random.Range(0, rooms.Length)]).transform.position = location;
        }
    }
}
