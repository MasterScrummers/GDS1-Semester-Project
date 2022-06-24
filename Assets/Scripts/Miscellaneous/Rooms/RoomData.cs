#pragma warning disable IDE1006 // Naming Styles
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class RoomData : MonoBehaviour
{
    private HealthComponent playerHP;
    [SerializeField] private Transform enemies; //The enemies in the room.
    [SerializeField] private GameObject[] itemsActiveOnClear; //All items turn ACTIVE when room is cleared.
    [SerializeField] private GameObject[] itemsInactiveOnClear; //All items turn INACTIVE when room is cleared.
    private List<GameObject> children = new(); //An array of the room's children (1 generation deep).
    private bool inRoom = false; //A boolean to check if the player is in the room.

    [field: SerializeField] public bool hasSetContent { get; private set; } = false;

    [field: Header("Room Exits"), SerializeField] public bool hasLeft { get; private set; } = true;
    [field: SerializeField] public bool hasRight { get; private set; } = true;
    [field: SerializeField] public bool hasUp { get; private set; } = true;
    [field: SerializeField] public bool hasDown { get; private set; } = true;

    public bool empty { get; private set; } //A boolean to check if the number of enemies is 0.
    private RoomContent roomContent;

    void Awake()
    {
        playerHP = DoStatic.GetPlayer<HealthComponent>();
        GetComponent<Collider2D>().isTrigger = true;

        if (!hasSetContent)
        {
            UpdateContent(DoStatic.GetGameController<VariableController>().GetRandomRoomContent());
        } else
        {
            UpdateChildren();
            ChildrenSetActive(false);
        }
    }

    void Update()
    {
        if (playerHP.health == 0 && enemies.gameObject.activeInHierarchy)
        {
            enemies.gameObject.SetActive(false);
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
        bool needUpdate = false;
        foreach(GameObject child in children)
        {
            needUpdate = child;
            if (child)
            {
                child.SetActive(setActive);
            }
        }
        if (needUpdate)
        {
            UpdateChildren();
        }
    }

    /// <summary>
    /// Updates the content of a room.
    /// </summary>
    /// <param name="newContent">The instantiated new room content.</param>
    /// <returns>True if successful update.</returns>
    public bool UpdateContent(RoomContent newContent)
    {
        if (hasSetContent)
        {
            return false;
        }

        if (roomContent)
        {
            Destroy(roomContent.gameObject);
        }
        roomContent = newContent;
        roomContent.transform.parent = transform;
        roomContent.transform.localPosition = Vector3.zero;
        enemies = roomContent.enemies;
        itemsActiveOnClear = roomContent.itemsActiveOnClear;
        UpdateChildren();
        ChildrenSetActive(inRoom);
        return true;
    }

    public void UpdateChildren()
    {
        children.Clear();
        List<string> ignore = new()
        {
            "Tilemaps",
            "Debug"
        };

        foreach(Transform child in DoStatic.GetChildren(transform))
        {
            if (!ignore.Contains(child.name))
            {
                children.Add(child.gameObject);
            }
        }
    }
}
