using System.Collections;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    private bool isTransitioning; //A check to know when the scene is transitioning.

    [SerializeField] private GameObject inGameUI; //To toggle it on/off.
    public GameObject player { get; private set; } //To toggle player on/off

    private UITransitionSystem transitionSystem; //To transition between scenes.
    private AudioController ac;

    /// <summary>
    /// All the scenes in the build. MUST MATCH NAME CORRECTLY.
    /// </summary>
    public enum SceneName
    {
        TitleScreen,
        Credits,
        OpeningCutscene,
        Tutorial,
        MainGame,
        DummyGame,
    };
    private SceneName currentScene; //The current scene's name

    private void Start()
    {
        player = DoStatic.GetChildWithTag("Player", transform);
        currentScene = (SceneName)System.Enum.Parse(typeof(SceneName), DoStatic.GetSceneName());
        transitionSystem = GetComponent<UIController>().GetUI<UITransitionSystem>("TransitionSystem");
        ac = GetComponent<AudioController>();
        GenericSceneStartUp(currentScene);
    }

    private void GenericSceneStartUp(SceneName sceneName)
    {
        bool isTitleScreen = (int)sceneName < 3;
        player.SetActive(!isTitleScreen);
        inGameUI.SetActive(!isTitleScreen);

        string[] mainGameTrackPool = new string[] {
            "Normal1", //Dance
            "Normal2", //Extreme Action (Might be better as a boss track??)
            "Normal3", //Sqz
        };

        ac.PlayMusic(sceneName switch
        {
            SceneName.TitleScreen => "TitleScreen", //Adventure
            SceneName.Credits => "Credits", //Enigmatic
            SceneName.OpeningCutscene => "Credits",
            _ => mainGameTrackPool[Random.Range(0, mainGameTrackPool.Length)],
        });
    }

    /// <summary>
    /// Changes the scene accordingly.
    /// </summary>
    /// <param name="notify">A method called once the new scene has finished coding.</param>
    /// <param name="sceneName">The new scene to load.</param>
    public void ChangeScene(SceneName sceneName, DoStatic.SimpleDelegate notify = null, string transitionName = "")
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(Transition(sceneName, notify, transitionName));
        }
    }

    /// <summary>
    /// Restarts the scene.
    /// </summary>
    /// <param name="notify">A method called once the new scene has finished coding.</param>
    /// <param name="transitionName"></param>
    public void RestartScene(DoStatic.SimpleDelegate notify = null, string transitionName = "")
    {
        ChangeScene(SceneName.MainGame, notify, transitionName);
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
        GenericSceneStartUp(currentScene);
        yield return StartCoroutine(LoadProgress(DoStatic.LoadScene(newSceneName.ToString())));
        notify?.Invoke();

        transitionSystem.Deactivate();
        yield return StartCoroutine(Wait(transitionSystem));
        isTransitioning = false;
    }
}
