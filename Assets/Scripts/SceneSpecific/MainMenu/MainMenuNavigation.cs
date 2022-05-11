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

    private enum Menu { Starting, Main, TutorialPrompt }
    private Menu menu = Menu.Starting;

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
                    menu++;
                    currentIndex = 0;
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
        switch(menu)
        {
            case Menu.Starting:
                start.SetActive(!spacePressed);
                menu = spacePressed ? menu + 1 : menu;
                return;

            case Menu.Main:
                if (spacePressed)
                {
                    DoOption();
                }
                bool isInMenu = menu == Menu.Main;
                pointer.gameObject.SetActive(isInMenu);
                optionList.SetActive(isInMenu);
                instructions.SetActive(isInMenu);
                currentIndex -= ic.GetButtonDown("MenuNavigation", "Vertical") ? (int)ic.GetAxisRawValues("MenuNavigation", "Vertical") : 0;
                currentIndex = currentIndex < 0 ? menuOptions.Length - 1 : currentIndex % menuOptions.Length;
                pointer.position = menuOptions[currentIndex].position;
                return;

            case Menu.TutorialPrompt:

                return;
        }

    }
}
