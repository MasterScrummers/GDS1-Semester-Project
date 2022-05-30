using UnityEngine;

public class PlayerPointer : MonoBehaviour
{
    private Transform player;
    void Start()
    {
        player = DoStatic.GetPlayer().transform;
    }

    void Update()
    {
        DoStatic.LookAt(transform, player);
    }
}
