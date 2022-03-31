using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public Dictionary<string, float> axisRawValues { get; private set; } //All inputs that checks Input.GetAxisRaw.
    public Dictionary<string, bool> buttonDowns { get; private set; } //All inputs that checks Input.GetButtonDown.
    public bool inputLock = false; //Bool to (un)lock inputs.

    private delegate Value InputCheck<Value>(string key); //Delegate variable for dynamic dictionary updates reusing code.

    private void Awake()
    {
        //Initiation + Default values

        //Add the inputs in the correct dictionary here!
        axisRawValues = new Dictionary<string, float>();
        foreach (string input in new string[] {
            "Horizontal", //[D, A]
            "Vertical" //[W, S]
        }) {
            axisRawValues.Add(input, 0);
        }

        buttonDowns = new Dictionary<string, bool>();
        foreach (string input in new string[]
        {
            "Jump", //Space
            "Light", //j
            "Heavy", //k
            "Special", //l
        }) {
            buttonDowns.Add(input, false);
        }
    }

    void Update()
    {
        if (inputLock)
        {
            return;
        }

        DictionaryUpdate(axisRawValues, GetAxisRaw);
        DictionaryUpdate(buttonDowns, GetButtonDown);
    }

    /// <summary>
    /// Enables/Disables all input.
    /// </summary>
    public void ToggleInputLock()
    {
        inputLock = !inputLock;
        if (inputLock)
        {
            InputReset();
        }
    }

    private void DictionaryUpdate<Value>(Dictionary<string, Value> dict, InputCheck<Value> check)
    {
        string[] keys = new string[dict.Keys.Count];
        dict.Keys.CopyTo(keys, 0);
        foreach (string key in keys)
        {
            dict[key] = check(key);
        }
    }

    private float GetAxisRaw(string key)
    {
        return Input.GetAxisRaw(key);
    }

    private bool GetButtonDown(string key)
    {
        return Input.GetButtonDown(key);
    }

    private void InputReset()
    {
        foreach(string key in axisRawValues.Keys)
        {
            axisRawValues[key] = 0;
        }

        foreach (string key in buttonDowns.Keys)
        {
            buttonDowns[key] = false;
        }
    }
}
