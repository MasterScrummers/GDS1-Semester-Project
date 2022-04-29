using UnityEngine;

public class MainMenuNavigation : MonoBehaviour
{
    private InputController ic;
    [SerializeField] private GameObject start;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private GameObject optionList;
    [SerializeField] private GameObject instructions;
    private RectTransform[] menuOptions;

    private int currentIndex = 0;

    private void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        Transform[] children = DoStatic.GetChildren(optionList.transform);
        menuOptions = new RectTransform[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            menuOptions[i] = children[i].GetComponent<RectTransform>();
        }
    }

    void Update()
    {
        void DoOption()
        {
            switch (menuOptions[currentIndex].name)
            {
                case "StartGame":
                    ic.GetComponent<SceneController>().ChangeScene(SceneController.SceneName.Tutorial);
                    return;

                case "Credits":
                    ic.GetComponent<SceneController>().ChangeScene(SceneController.SceneName.Credits);
                    return;

                case "Exit":
                    Application.Quit();
                    return;
            }
        }

        bool spacePressed = Input.GetKeyDown(KeyCode.Space);
        if (start.activeInHierarchy)
        {
            start.SetActive(!spacePressed);

            bool startIsActive = start.activeInHierarchy;
            pointer.gameObject.SetActive(!startIsActive);
            optionList.SetActive(!startIsActive);
            instructions.SetActive(!startIsActive);
        } else if (spacePressed)
        {
            DoOption();
            return;
        }

        currentIndex -= ic.GetButtonDown("MenuNavigation", "Vertical") ? (int)ic.GetAxisRawValues("MenuNavigation", "Vertical") : 0;
        currentIndex = currentIndex < 0 ? menuOptions.Length - 1 : currentIndex % menuOptions.Length;
        pointer.position = menuOptions[currentIndex].position;
    }
}
