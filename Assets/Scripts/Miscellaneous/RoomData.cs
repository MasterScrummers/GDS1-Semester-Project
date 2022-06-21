#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomData : MonoBehaviour
{
    private HealthComponent playerHP;
    [SerializeField] private Transform enemies; //The enemies in the room.
    [SerializeField] private GameObject[] itemsActiveOnClear; //All items turn ACTIVE when room is cleared.
    [SerializeField] private GameObject[] itemsInactiveOnClear; //All items turn INACTIVE when room is cleared.
    private GameObject[] children; //An array of the room's children (1 generation deep).
    private bool inRoom = false; //A boolean to check if the player is in the room.

    public enum Direction { left, right, up, down }
    [Header("Room Exits")]
    [SerializeField] private bool hasLeft = true;
    [SerializeField] private bool hasRight = true;
    [SerializeField] private bool hasUp = true;
    [SerializeField] private bool hasDown = true;

    public bool empty { get; private set; } //A boolean to check if the number of enemies is 0.

    void Awake()
    {
        playerHP = DoStatic.GetPlayer<HealthComponent>();
        GetComponent<Collider2D>().isTrigger = true;

        Transform[] toddlers = DoStatic.GetChildren(transform);
        children = new GameObject[toddlers.Length];
        for (int i = 0; i < toddlers.Length; i++)
        {
            children[i] = toddlers[i].gameObject;
        }
        ChildrenSetActive(false);
    }

    void Update()
    {
        if (playerHP.health == 0 && enemies.gameObject.activeInHierarchy)
        {
            ChildrenSetActive(false);
            inRoom = false;
        }

        if (!inRoom)
        {
            return;
        }

        empty = IsEmpty();
        foreach (GameObject gameObject in itemsActiveOnClear)
        {
            gameObject.SetActive(empty);
        }

        foreach (GameObject gameObject in itemsInactiveOnClear)
        {
            gameObject.SetActive(!empty);
        }
    }

    private bool IsEmpty()
    {
        for (int i = 0; i < enemies.childCount; i++)
        {
            GameObject child = enemies.GetChild(i).gameObject;
            if (!child.activeInHierarchy)
            {
                Destroy(child);
                continue;
            }
            break;
        }
        return enemies.childCount == 0;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
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
