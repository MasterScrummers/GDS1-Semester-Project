using UnityEngine;

public class MoveRooms : MonoBehaviour
{
    [SerializeField] private int doorPos;
    private GameObject kirby;

    void Start()
    {
        kirby = DoStatic.GetPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            MovePlayerRoom();
        }
    }

    private void MovePlayerRoom()
    {
        switch(doorPos)
        {
            case 0:
                kirby.transform.position += new Vector3(0f, 38f, 0f);
                return;

            case 1:
                kirby.transform.position += new Vector3(35.5f, 0f, 0f);
                return;

            case 2:
                kirby.transform.position += new Vector3(0f, -38f, 0f);
                return;

            case 3:
                kirby.transform.position += new Vector3(-35.5f, 0f, 0f);
                return;
        }
    }
}
