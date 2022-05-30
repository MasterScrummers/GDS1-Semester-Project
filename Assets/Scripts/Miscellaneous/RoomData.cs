using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomData : MonoBehaviour
{
    [SerializeField] private GameObject enemies; //The enemies in the room.
    [SerializeField] private GameObject doors; //The doors in the room.
    [SerializeField] private GameObject chest; //The chest in the room. (Optional to have one.)
    [SerializeField] private GameObject nextLevelDoor; //The next level door in the room. (Optional to have one.)
    private GameObject[] children; //An array of the room's children (1 generation deep).
    private bool inRoom = false; //A boolean to check if the player is in the room.

    public enum Direction { left, right, up, down }
    [Header("Room Exits")]
    [SerializeField] private bool hasLeft = true;
    [SerializeField] private bool hasRight = true;
    [SerializeField] private bool hasUp = true;
    [SerializeField] private bool hasDown = true;

    public bool empty { get; private set; } //A boolean to check if the number of enemies is 0.
    private int enemyCount; //Tracks the number of enemies in the room.

    void Awake()
    {
        GetComponent<Collider2D>().isTrigger = true;

        Transform[] toddlers = DoStatic.GetChildren(transform);
        children = new GameObject[toddlers.Length];
        for (int i = 0; i < toddlers.Length; i++)
        {
            children[i] = toddlers[i].gameObject;
        }

        foreach(Transform child in DoStatic.GetChildren(enemies.transform))
        {
            Enemy enemy = child.GetComponent<Enemy>();
            if (enemy && child.gameObject.activeInHierarchy)
            {
                enemy.AssignToRoomData(this);
                enemyCount++;
            }
        }
        ChildrenSetActive(false);
    }

    void Update()
    {
        void CheckSetActive(ref GameObject check, bool active)
        {
            if (check)
            {
                check.SetActive(active);
            }
        }

        if (!inRoom)
        {
            return;
        }

        empty = enemyCount == 0;
        CheckSetActive(ref doors, !empty);
        CheckSetActive(ref chest, empty);
        CheckSetActive(ref nextLevelDoor, empty);

    }

    public void UpdateEnemyCount()
    {
        enemyCount--;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Vector3 camPos = Camera.main.transform.position;
            Vector3 pos = transform.position;
            camPos.x = pos.x;
            camPos.y = pos.y;
            Camera.main.transform.position = camPos;
            ChildrenSetActive(true);
            inRoom = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            ChildrenSetActive(false);
            inRoom = false;
        }
    }

    private void ChildrenSetActive(bool setActive)
    {
        foreach(GameObject child in children)
        {
            if (!child.name.Equals("Tilemaps"))
            {
                child.SetActive(setActive);
            }
        }
    }

    public bool HasExit(Direction dir)
    {
        return dir switch
        {
            Direction.left => hasLeft,
            Direction.right => hasRight,
            Direction.up => hasUp,
            Direction.down => hasDown,
            _ => throw new System.NotImplementedException()
        };
    }
}
