using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    private GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = DoStatic.GetPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = player.transform.position;
    }
}
