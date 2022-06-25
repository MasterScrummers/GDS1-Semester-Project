using UnityEngine;

public class RoomContent : MonoBehaviour
{
    [field: SerializeField] public Transform enemies { get; private set; } //The enemies in the room.
    [field: SerializeField] public GameObject[] itemsActiveOnClear { get; private set; } //All items turn ACTIVE when room is cleared.
    [field: SerializeField] public GameObject tilemaps { get; private set; }
    [SerializeField] private GameObject DestroyOnAwake;

    private void Awake()
    {
        Destroy(DestroyOnAwake);
    }
}
