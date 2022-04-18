using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    private Dictionary<string, UISystemBase> UIs; //A dictionary of all the UIs
    [SerializeField] private UISystemBase[] UISystems; //A list to add into the dictionary.

    public float transitionSpeed = 1;

    void Start()
    {
        UIs = new Dictionary<string, UISystemBase>();
        foreach (UISystemBase system in UISystems)
        {
            UIs.Add(system.name, system);
            system.StartUp();
            system.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Activates the UI.
    /// </summary>
    /// <param name="name">The name of the UI.</param>
    /// <param name="notification">The method that will be called once it has transitioned in..</param>
    public void ActivateUI(string name, DoStatic.SimpleDelegate notification)
    {
        UISystemBase uISystem = UIs[name];
        uISystem.gameObject.SetActive(true);
        uISystem.TransitionIn(transitionSpeed, notification);
    }

    public void DeactivateUI(string name)
    {
        UISystemBase uISystem = UIs[name];
        if (uISystem.gameObject.activeInHierarchy)
        {
            uISystem.TransitionOut(transitionSpeed);
        }
    }

    public T GetUI<T>(string name) where T : UISystemBase
    {
        return (T)UIs[name];
    }
}
