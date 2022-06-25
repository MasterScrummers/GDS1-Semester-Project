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
            Vector3 sca = transform.localScale;
            sca.x = horizontal;
            transform.localScale = sca;
        }
    }
}
