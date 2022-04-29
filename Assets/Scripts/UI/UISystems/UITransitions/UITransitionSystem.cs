using System.Collections.Generic;
using UnityEngine;

public class UITransitionSystem : UISystemBase
{
    [SerializeField] private UITransitionBase[] transitions;
    private Dictionary<string, UITransitionBase> loadingScreens; //A dictionary of all the UIs
    private UITransitionBase activeScreen;

    public override void Initiate()
    {
        base.Initiate();
        loadingScreens = new Dictionary<string, UITransitionBase>();
        foreach (UITransitionBase transition in transitions)
        {
            loadingScreens.Add(transition.name, transition);
            transition.Initiate();
            transition.gameObject.SetActive(false);
        }
        SetRandomTransition();
    }

    public override void Activate(DoStatic.SimpleDelegate notify = null)
    {
        base.Activate(notify);
        ic.SetInputLock(true);
        
        activeScreen.Activate(transitionTime, notify);
    }

    public override void Deactivate(DoStatic.SimpleDelegate notify = null)
    {
        base.Deactivate(notify);
        activeScreen.Deactivate(transitionTime, TurnOff);
    }

    private void TurnOff()
    {
        Notify();
        activeScreen.gameObject.SetActive(false);
        SetRandomTransition();
        ic.SetInputLock(false);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Specify a transition name. If the transition doesn't exist, it will be randomised.
    /// </summary>
    /// <param name="name">The name of the transition.</param>
    public void SetTransition(string name)
    {
        if (loadingScreens.ContainsKey(name))
        {
            activeScreen = loadingScreens[name];
        }
    }

    private void SetRandomTransition()
    {
        string[] transitions = new string[loadingScreens.Count];
        loadingScreens.Keys.CopyTo(transitions, 0);
        SetTransition(transitions[Random.Range(0, transitions.Length)]);
    }

    /// <summary>
    /// Get the current status of the transition.
    /// </summary>
    /// <returns>True if the transition is fully in/out.</returns>
    public bool IsTransitionReady()
    {
        return activeScreen.isReady;
    }
}
