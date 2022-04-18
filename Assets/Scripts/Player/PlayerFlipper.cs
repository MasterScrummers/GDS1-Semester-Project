using UnityEngine;

public class PlayerFlipper : MonoBehaviour
{
    private InputController ic; //To get horizonal.

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
    }

    void Update()
    {
        float horizontal = ic.GetAxisRawValues("Movement", "Horizontal");
        if (horizontal != 0)
        {
            Vector3 rot = transform.eulerAngles;
            rot.y = horizontal == 1 ? 0 : 180;
            transform.eulerAngles = rot;
        }
    }
}
