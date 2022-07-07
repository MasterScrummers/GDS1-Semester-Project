using UnityEngine;

public class MainMenuNavigation : MonoBehaviour
{
    private InputController ic;
    private AudioController ac;
    [SerializeField] private GameObject start;
    [SerializeField] private RectTransform pointer;
    [SerializeField] private GameObject optionList;
    [SerializeField] private GameObject instructions;
    [SerializeField] private GameObject tutorialPrompt;
    private RectTransform[] menuOptions;
    private RectTransform[] promptOptions;

    private int currentIndex = 0;

    private enum Menu { Starting, Main, TutorialPrompt }
    private Menu menu = Menu.Starting;

    private void Start()
    {
        ic = DoStatic.GetGameController<InputController>();
        ac = DoStatic.GetGameController<AudioController>();
        menuOptions = GetOptions(DoStatic.GetChildren(optionList.transform));
        promptOptions = GetOptions(DoStatic.GetChildren(tutorialPrompt.transform));

        ic.GetComponent<VariableController>().ResetLevel();
        Vector3 camPos = Vector3.zero;
        camPos.z = -10;
        Camera.main.transform.position = camPos;

        HealthComponent health = ic.GetComponent<SceneController>().player.GetComponent<HealthComponent>();
        health.SetHP();
        health.GetComponent<PlayerInput>().Restart();
    }

    private RectTransform[] GetOptions(Transform[] children)
    {
        RectTransform[] options = new RectTransform[children.Length];
        for (int i = 0; i < children.Length; i++)
        {
            options[i] = children[i].GetComponent<RectTransform>();
        }
        return options;
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

        void SetPointerPosition(int increments, RectTransform[] options)
        {
            if (increments != 0) {ac.PlaySound("MenuSelect");}
            currentIndex += increments;
            currentIndex = currentIndex < 0 ? options.Length - 1 : currentIndex % options.Length;
            pointer.position = options[currentIndex].position;
        }

        void DoGameOption()
        {
            if (promptOptions[currentIndex].name.Equals("Yes"))
            {
                VariableController var = ic.GetComponent<VariableController>();
                var.SetScene(SceneController.SceneName.Tutorial);
            }

            ic.GetComponent<SceneController>().ChangeScene(SceneController.SceneName.OpeningCutscene);
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
                optionList.SetActive(menu == Menu.Main);
                pointer.gameObject.SetActive(true);
                instructions.SetActive(true);

                SetPointerPosition(ic.GetButtonDown("MenuNavigation", "Vertical") ? (int)-ic.GetAxisRawValues("MenuNavigation", "Vertical") : 0, menuOptions);
                return;

            case Menu.TutorialPrompt:
                if (spacePressed)
                {
                    DoGameOption();
                }
                tutorialPrompt.SetActive(true);
                SetPointerPosition(ic.GetButtonDown("MenuNavigation", "Horizontal") ? (int)ic.GetAxisRawValues("MenuNavigation", "Horizontal") : 0, promptOptions);
                return;
        }

        
    }
}
