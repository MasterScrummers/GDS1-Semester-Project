using UnityEngine;

public class MoveRooms : MonoBehaviour
{
    private GameObject gameController;
    [SerializeField] private int doorPos;
    private GameObject kirby;

    void Start()
    {
        gameController = DoStatic.GetGameController();
        kirby = DoStatic.GetPlayer();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
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
                gameController.transform.position += new Vector3(0f, 50f, 0f);
                return;

            case 1:
                kirby.transform.position += new Vector3(35.5f, 0f, 0f);
                gameController.transform.position += new Vector3(50f, 0f, 0f);
                return;

            case 2:
                kirby.transform.position += new Vector3(0f, -38f, 0f);
                gameController.transform.position += new Vector3(0f, -50f, 0f);
                return;

            case 3:
                kirby.transform.position += new Vector3(-35.5f, 0f, 0f);
                gameController.transform.position += new Vector3(-50f, 0f, 0f);
                return;
        }
    }
}
