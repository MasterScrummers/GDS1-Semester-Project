using System.Collections;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private bool isTransitioning; //A check to know when the scene is transitioning.
    
    [SerializeField] private GameObject inGameUI; //To toggle it on/off.
    private GameObject player; //To toggle player on/off

    private UITransitionSystem transitionSystem; //To transition between scenes.

    /// <summary>
    /// All the scenes in the build. MUST MATCH NAME CORRECTLY.
    /// </summary>
    public enum SceneName
    {
        TitleScreen,
        Credits,
        Tutorial,
        MainGame,
    };
    private SceneName currentScene; //The current scene's name

    void Start()
    {
        player = DoStatic.GetChildWithTag("Player", transform);
        currentScene = (SceneName)System.Enum.Parse(typeof(SceneName), DoStatic.GetSceneName());
        transitionSystem = GetComponent<UIController>().GetUI<UITransitionSystem>("TransitionSystem");
        GenericSceneStartUp();
    }
    
    private void GenericSceneStartUp()
    {
        bool isTitleScreen = false;
        foreach(SceneName sceneName in new SceneName[] { SceneName.TitleScreen, SceneName.Credits })
        {
            if (isTitleScreen = currentScene == sceneName)
            {
                break;
            }
        }

        player.SetActive(!isTitleScreen);
        inGameUI.SetActive(!isTitleScreen);
    }

    /// <summary>
    /// Changes the scene accordingly.
    /// </summary>
    /// <param name="sceneName">The new scene to load.</param>
    public void ChangeScene(SceneName sceneName, DoStatic.SimpleDelegate notify = null, string transitionName = "")
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(Transition(sceneName, notify, transitionName));
        }
    }

    private IEnumerator Transition(SceneName newSceneName, DoStatic.SimpleDelegate notify, string transitionName)
    {
        IEnumerator LoadProgress(AsyncOperation async)
        {
            while (!async.isDone)
            {
                async.allowSceneActivation = async.progress >= 0.9f;
                yield return null;
            }
        }

        IEnumerator Wait(UITransitionSystem transitionSystem)
        {
            while (!transitionSystem.IsTransitionReady())
            {
                yield return null;
            }
        }

        transitionSystem.SetTransition(transitionName);
        transitionSystem.Activate();
        yield return StartCoroutine(Wait(transitionSystem));

        currentScene = newSceneName;
        GenericSceneStartUp();
        yield return StartCoroutine(LoadProgress(DoStatic.LoadScene(newSceneName.ToString())));
        if (notify != null)
        {
            notify();
        }

        transitionSystem.Deactivate();
        yield return StartCoroutine(Wait(transitionSystem));
        isTransitioning = false;
    }
}
