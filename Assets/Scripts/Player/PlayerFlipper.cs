using UnityEngine;

public class PlayerFlipper : MonoBehaviour
{
    private InputController ic; //To get horizonal.

    void Start()
    {
        ic = DoStatic.GetGameController().GetComponent<InputController>();
    }

    void Update()
    {
        float horizontal = ic.axisRawValues["Horizontal"];
        if (horizontal != 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = horizontal;
            transform.localScale = scale;
        }
    }
}
