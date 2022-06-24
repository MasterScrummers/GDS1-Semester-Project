using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [SerializeField] private RoomData[] rooms; //Not yet sorted into a dictionary.

    private readonly Dictionary<Vector2, RoomData> grid = new();
    private readonly Dictionary<string, List<GameObject>> sortedNormalRooms = new();

    private void Start()
    {
        foreach (string combination in new string[]
        {
            "L", "R", "U", "D", "",

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
            sortedNormalRooms[GetExits(room)].Add(room.gameObject);
        }
    }

    /// <summary>
    /// To be called ONCE per level by external means (usually by SceneStartUp).
    /// </summary>
    public void GenerateLevel(int maxRooms, RoomContent[] roomContents)
    {
        PlotRooms(maxRooms, new(0, 0));
        RemoveSomeRooms();
        GenerateRooms();
        FixDisconnectedRooms();
        InsertMandatoryContents(roomContents);
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

            Vector2[] corners = new Vector2[] {
                location + new Vector2(50, 50), // topright
                location + new Vector2(-50, -50), //BottomLeft
                location + new Vector2(-50, 50), //TopLeft
                location + new Vector2(50, -50), //BottomRight
            };
            
            bool positiveDiagonal = grid.ContainsKey(corners[0]) && grid.ContainsKey(corners[1]);
            bool negativeDiagonal = grid.ContainsKey(corners[2]) && grid.ContainsKey(corners[3]);

            if (positiveDiagonal || negativeDiagonal)
            {
                grid.Remove(location);

                bool isOk = true;
                foreach (var corner in corners)
                {
                    isOk = isOk && ConnectionCheck(corner);
                }

                if (!isOk)
                {
                    grid.Add(location, null);
                    return;
                }
                Instantiate(sortedNormalRooms[""][0]).transform.position = location;
            }
        }
    }

    private bool ConnectionCheck(Vector2 pos)
    {
        void AddToPlacesToCheck(Vector2 pos, HashSet<Vector2> history, Queue<Vector2> placesToCheck)
        {
            if (grid.ContainsKey(pos) && !history.Contains(pos) && !placesToCheck.Contains(pos))
            {
                placesToCheck.Enqueue(pos);
            }
        }

        if (!grid.ContainsKey(pos))
        {
            return true;
        }

        HashSet<Vector2> history = new();
        Queue<Vector2> placesToCheck = new();
        placesToCheck.Enqueue(pos);

        while (placesToCheck.Count > 0)
        {
            Vector2 location = placesToCheck.Dequeue();
            history.Add(location);

            AddToPlacesToCheck(location + new Vector2(-50, 0), history, placesToCheck);
            AddToPlacesToCheck(location + new Vector2(50, 0), history, placesToCheck);
            AddToPlacesToCheck(location + new Vector2(0, 50), history, placesToCheck);
            AddToPlacesToCheck(location + new Vector2(0, -50), history, placesToCheck);
        }

        return history.Count == grid.Count;
    }

    private string CheckAndSpawnNeighbour(Vector2 neighbour, bool checkVertically, string neighbourDirection)
    {
        if (!grid.ContainsKey(neighbour))
        {
            return "";
        }

        RoomData room = grid[neighbour];
        if (room == null)
        {
            RoomData adjNeighbourA;
            RoomData adjNeighbourB;
            Vector2 adjPos;

            if (checkVertically)
            {
                adjPos = neighbour + new Vector2(0, 50);
                adjNeighbourA = grid.ContainsKey(adjPos) ? grid[adjPos] : null; //Up

                adjPos = neighbour + new Vector2(0, -50);
                adjNeighbourB = grid.ContainsKey(adjPos) ? grid[adjPos] : null; //Down
            }
            else
            {
                adjPos = neighbour + new Vector2(50, 0);
                adjNeighbourA = grid.ContainsKey(adjPos) ? grid[adjPos] : null; //Right

                adjPos = neighbour + new Vector2(-50, 0);
                adjNeighbourB = grid.ContainsKey(adjPos) ? grid[adjPos] : null; //Left
            }

            return neighbourDirection switch
            {
                "L" => adjNeighbourA && adjNeighbourA.hasDown || adjNeighbourB && adjNeighbourB.hasUp,
                "R" => adjNeighbourA && adjNeighbourA.hasDown || adjNeighbourB && adjNeighbourB.hasUp,
                "U" => adjNeighbourA && adjNeighbourA.hasLeft || adjNeighbourB && adjNeighbourB.hasRight,
                "D" => adjNeighbourA && adjNeighbourA.hasLeft || adjNeighbourB && adjNeighbourB.hasRight,
                _ => false,
            } && DoStatic.RandomBool() ? "" : neighbourDirection;
        }

        return neighbourDirection switch
        {
            "L" => room.hasRight,
            "R" => room.hasLeft,
            "U" => room.hasDown,
            "D" => room.hasUp,
            _ => false,
        } ? neighbourDirection : "";
    }

    private string ConnectToAllNeighbours(Vector2 pos)
    {
        string roomType = grid.ContainsKey(pos + new Vector2(-50, 0)) ? "L" : "";
        roomType += grid.ContainsKey(pos + new Vector2(50, 0)) ? "R" : "";
        roomType += grid.ContainsKey(pos + new Vector2(0, 50)) ? "U" : "";
        roomType += grid.ContainsKey(pos + new Vector2(0, -50)) ? "D" : "";
        return roomType;
    }

    private void GenerateRooms()
    {
        Vector2[] locations = new Vector2[grid.Count];
        grid.Keys.CopyTo(locations, 0);
        DoStatic.ShuffleArray(locations);

        foreach (Vector2 location in locations)
        {
            string roomType = CheckAndSpawnNeighbour(location + new Vector2(-50, 0), true, "L"); //Left neighbour
            roomType += CheckAndSpawnNeighbour(location + new Vector2(50, 0), true, "R"); //Right neighbour
            roomType += CheckAndSpawnNeighbour(location + new Vector2(0, 50), false, "U"); //Up neighbour
            roomType += CheckAndSpawnNeighbour(location + new Vector2(0, -50), false, "D"); //Down neighbour
            roomType = roomType.Equals("") ? ConnectToAllNeighbours(location) : roomType;
            GenerateRoom(roomType, location);
        }
    }

    private void GenerateRoom(string roomType, Vector2 location)
    {
        List<GameObject> roomPool = sortedNormalRooms[roomType];
        RoomData room = Instantiate(roomPool[Random.Range(0, roomPool.Count)]).GetComponent<RoomData>();
        room.transform.position = location;
        grid[location] = room;
    }

    private void FixDisconnectedRooms()
    {
        Vector2[] locations = new Vector2[grid.Count];
        grid.Keys.CopyTo(locations, 0);
        foreach (Vector2 location in locations)
        {
            bool traversalCheck = grid.ContainsKey(location + new Vector2(-50, 0)) && !grid[location].hasLeft;
            traversalCheck = traversalCheck || grid.ContainsKey(location + new Vector2(50, 0)) && !grid[location].hasRight;
            traversalCheck = traversalCheck || grid.ContainsKey(location + new Vector2(0, 50)) && !grid[location].hasUp;
            traversalCheck = traversalCheck || grid.ContainsKey(location + new Vector2(0, -50)) && !grid[location].hasDown;
            if (traversalCheck && !CanTraverseToOrigin(grid[location], grid[new(0, 0)]))
            {
                DestroyAndRegenerateRoom(location);
            }
        }
    }

    private void DestroyAndRegenerateRoom(Vector2 location)
    {
        void UpdateNeighbour(bool update, Vector2 location, string mustConnect)
        {
            if (!update || !grid.ContainsKey(location))
            {
                return;
            }

            RoomData room = grid[location];
            string exits = room.hasLeft || mustConnect.Equals("L") ? "L" : "";
            exits += room.hasRight || mustConnect.Equals("R") ? "R" : "";
            exits += room.hasUp || mustConnect.Equals("U") ? "U" : "";
            exits += room.hasDown || mustConnect.Equals("D") ? "D" : "";
            Destroy(room.gameObject);
            GenerateRoom(exits, location);
        }

        string roomType = GetExits(grid[location]);

        UpdateNeighbour(!roomType.Contains("L"), location + new Vector2(-50, 0), "R");
        UpdateNeighbour(!roomType.Contains("R"), location + new Vector2(50, 0), "L");
        UpdateNeighbour(!roomType.Contains("U"), location + new Vector2(0, 50), "D");
        UpdateNeighbour(!roomType.Contains("D"), location + new Vector2(0, -50), "U");

        Destroy(grid[location].gameObject);
        GenerateRoom(ConnectToAllNeighbours(location), location);
    }

    private bool CanTraverseToOrigin(RoomData start, RoomData dest)
    {
        void AddToPlacesToCheck(bool hasNeighbour, Vector2 location, HashSet<RoomData> history, Queue<RoomData> placesToCheck)
        {
            RoomData place = grid.ContainsKey(location) ? grid[location] : null;
            if (hasNeighbour && place && !history.Contains(place) && !placesToCheck.Contains(place))
            {
                placesToCheck.Enqueue(place);
            }
        }

        if (start == dest)
        {
            return true;
        }

        HashSet<RoomData> history = new();
        Queue<RoomData> placesToCheck = new();
        placesToCheck.Enqueue(start);
        while (placesToCheck.Count > 0)
        {
            RoomData current = placesToCheck.Dequeue();
            history.Add(current);

            if (current == dest)
            {
                return true;
            }

            Vector2 pos = current.transform.position;
            AddToPlacesToCheck(current.hasLeft, pos + new Vector2(-50, 0), history, placesToCheck);
            AddToPlacesToCheck(current.hasRight, pos + new Vector2(50, 0), history, placesToCheck);
            AddToPlacesToCheck(current.hasUp, pos + new Vector2(0, 50), history, placesToCheck);
            AddToPlacesToCheck(current.hasDown, pos + new Vector2(0, -50), history, placesToCheck);
        }
        return false;
    }

    private string GetExits(RoomData room)
    {
        string exits = room.hasLeft ? "L" : "";
        exits += room.hasRight ? "R" : "";
        exits += room.hasUp ? "U" : "";
        exits += room.hasDown ? "D" : "";
        return exits;
    }

    private void InsertMandatoryContents(RoomContent[] roomContents)
    {
        Vector2[] locations = new Vector2[grid.Count];
        grid.Keys.CopyTo(locations, 0);
        DoStatic.ShuffleArray(locations);

        int keyCount = 0;
        for (int i = 0; i < roomContents.Length; i++)
        {
            RoomContent content = Instantiate(roomContents[i]);
            Vector2 location;
            while ((location = locations[keyCount++]) == Vector2.zero || !grid[location].UpdateContent(content)) {}
        }
    }
}
