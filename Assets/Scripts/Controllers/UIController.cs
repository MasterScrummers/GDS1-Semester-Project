using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private UISystemBase[] UISystems; //A list to add into the dictionary.
    private Dictionary<string, UISystemBase> UIs; //A dictionary of all the UIs

    void Awake()
    {
        UIs = new Dictionary<string, UISystemBase>();
        foreach (UISystemBase system in UISystems)
        {
            UIs.Add(system.name, system);
            system.Initiate();
            system.gameObject.SetActive(false);
        }
    }

    public T GetUI<T>(string name) where T : UISystemBase
    {
        return (T)UIs[name];
    }
}
