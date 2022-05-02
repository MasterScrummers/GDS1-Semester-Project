using UnityEngine;

public class CreditEscape : MonoBehaviour
{
    private InputController ic;

    void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
    }

    void Update()
    {
        if (ic.GetButtonDown("Credits", "Exit"))
        {
            ic.GetComponent<SceneController>().ChangeScene(SceneController.SceneName.TitleScreen);
        }
    }
}
