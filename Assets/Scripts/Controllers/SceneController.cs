using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController : MonoBehaviour
{
    /// <summary>
    /// TransitionTime is a internal class that turns into a dictionary on awake.
    /// Dictonaries cannot be edited on the Unity Editor. Hence the internal class.
    /// </summary>
    [System.Serializable]
    public class TransitionTime
    {
        public string transitionName; //Transition's name
        public float timeIn; //The time for the transition to get in.
        public float timeOut; //The time for the transition to get out.

        /// <param name="name">Name of the transition.</param>
        /// <param name="transitionTimeIn">Speed of the transition going in.</param>
        /// <param name="transitionTimeOut">Speed of the transition going out.</param>
        public TransitionTime(string name, float transitionTimeIn, float transitionTimeOut)
        {
            transitionName = name;
            timeIn = transitionTimeIn;
            timeOut = transitionTimeOut;
        }
    }

    private string currentScene; //The current scene's name

    private TransitionBase currentTransition; //The current transition that is happening.
    private bool isTransitioning; //A check to know when the scene is transitioning.

    public TransitionTime[] allTansitionTimes; //All the transition times (can be added/edited on the editor).
    private Dictionary<string, float[]> transitionTimes; //The transition times when changing scenes.

    private void Awake()
    {
        transitionTimes = new Dictionary<string, float[]>();
        foreach (TransitionTime t in allTansitionTimes)
        {
            transitionTimes.Add(t.transitionName, new float[] { t.timeIn, t.timeOut });
        }
    }

    void Start()
    {
        currentScene = DoStatic.GetSceneName();
    }

    /// <summary>
    /// Should only be called from a TransitionBase class.
    /// </summary>
    /// <param name="transition">Should be the current transition.</param>
    /// <returns>A float of the times.</returns>
    public float[] GetTransitionTimes(TransitionBase transition)
    {
        currentTransition = transition;
        string transitionName = transition.transitionName;
        return transitionTimes.ContainsKey(transitionName) ? transitionTimes[transitionName] : new float[] { 1, 1 };
    }

    /// <summary>
    /// Changes the scene accordingly.
    /// </summary>
    /// <param name="sceneName">The new scene to load.</param>
    public void ChangeScene(string sceneName, string transitionName = "Fade")
    {
        if (!isTransitioning)
        {
            isTransitioning = true;
            StartCoroutine(Transition(transitionName, sceneName));
        }
    }

    private IEnumerator Transition(string transitionName, string newSceneName)
    {
        DoStatic.LoadScene(transitionName);
        yield return StartCoroutine(Wait("LoadingTransition"));
        currentTransition.InitiateTransition();
        yield return StartCoroutine(Wait("TransitionSlowIn"));

        DoStatic.UnloadScene(currentScene);
        AsyncOperation op = DoStatic.LoadScene(newSceneName);
        if (op.progress != 1)
        {
            yield return null;
        }

        currentScene = newSceneName;

        currentTransition.startFinishingUp = true;
        yield return StartCoroutine(Wait("TransitionSlowOut"));
        DoStatic.UnloadScene(transitionName);
        yield return StartCoroutine(Wait("UnloadingTransition"));
        isTransitioning = false;
    }

    private bool WaitCheck(string reason)
    {
        switch (reason)
        {
            case "LoadingTransition":
                return !currentTransition;

            case "TransitionSlowIn":
                return !currentTransition.startLoading;

            case "TransitionSlowOut":
                return currentTransition.startFinishingUp;

            case "UnloadingTransition":
                return currentTransition;
        }
        return true;
    }

    private IEnumerator Wait(string reason)
    {
        yield return new WaitForEndOfFrame();
        while (WaitCheck(reason))
        {
            yield return null;
        }
    }
}
