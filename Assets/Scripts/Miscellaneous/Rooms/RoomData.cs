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

    [field: SerializeField] public bool hasContent { get; private set; } = false;

    [field: Header("Room Exits"), SerializeField] public bool hasLeft { get; private set; } = true;
    [field: SerializeField] public bool hasRight { get; private set; } = true;
    [field: SerializeField] public bool hasUp { get; private set; } = true;
    [field: SerializeField] public bool hasDown { get; private set; } = true;

    public bool empty { get; private set; } //A boolean to check if the number of enemies is 0.

    void Awake()
    {
        playerHP = DoStatic.GetPlayer<HealthComponent>();
        GetComponent<Collider2D>().isTrigger = true;

        if (!hasContent)
        {
            RoomContent roomContent = DoStatic.GetGameController<VariableController>().GetRandomRoomContent();
            roomContent.transform.parent = transform;
            roomContent.transform.localPosition = Vector3.zero;
            enemies = roomContent.enemies;
            itemsActiveOnClear = roomContent.itemsActiveOnClear;
        }

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

        empty = enemies.childCount == 0;
        foreach (GameObject gameObject in itemsActiveOnClear)
        {
            gameObject.SetActive(empty);
        }

        foreach (GameObject gameObject in itemsInactiveOnClear)
        {
            gameObject.SetActive(!empty);
        }
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
}
