using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool lockedInput = false; //Bool to (un)lock all inputs.
    private Dictionary<string, bool> IDList; //Allows inputs depending on the boolean set. Has to be part of IDList to work.

    private Dictionary<string, float> axisRawValues; //All inputs that checks Input.GetAxisRaw.
    private Dictionary<string, bool> buttonDowns; //All inputs that checks Input.GetButtonDown.
    private Dictionary<string, bool> buttonStates; //All inputs that checks Input.GetButton.

    private delegate Value InputCheck<Value>(string key); //Delegate variable for dynamic dictionary updates reusing code.

    private void Awake()
    {
        //Initiation + Default values
        IDList = new Dictionary<string, bool>();
        foreach(string input in new string[] { //Treat the ID list like they are GameObject tags
            "Attack", //Related to attack inputs.
            "Movement", //Related to movement
            "WeaponSwap" //The weapon swap system
        })
        {
            IDList.Add(input, true);
        }

        //Add the inputs in the correct dictionary here!
        axisRawValues = new Dictionary<string, float>();
        foreach (string input in new string[] {
            "Horizontal", //[D, A, left, right]
            "Vertical" //[W, S, up, down]
        }) {
            axisRawValues.Add(input, GetAxisRaw(input));
        }

        buttonDowns = new Dictionary<string, bool>();
        buttonStates = new Dictionary<string, bool>();
        foreach (string input in new string[]
        {
            "Jump", //space, w
            "Light", //j
            "Heavy", //k
            "Special", //l
            "Interact", //w
            "Exit"
        }) {
            buttonDowns.Add(input, GetButtonDown(input));
            buttonStates.Add(input, GetButtonState(input));
        }
    }

    void Update()
    {
        DictionaryUpdate(ref axisRawValues, GetAxisRaw);
        DictionaryUpdate(ref buttonDowns, GetButtonDown);
        DictionaryUpdate(ref buttonStates, GetButtonState);
    }

    private void DictionaryUpdate<Value>(ref Dictionary<string, Value> dict, InputCheck<Value> check)
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

    private bool GetButtonState(string key)
    {
        return Input.GetButton(key);
    }

    /// <summary>
    /// Enables/Disables all input.
    /// Perfect for cutscenes.
    /// </summary>
    public void SetInputLock(bool doLock)
    {
        lockedInput = doLock;
    }

    /// <summary>
    /// Adds a reason to the blacklist.
    /// Makes InputController return negative for that reason.
    /// </summary>
    /// <param name="ID">The reason</param>
    public void SetID(string ID, bool state)
    {
        if (IDList.ContainsKey(ID))
        {
            IDList[ID] = state;
        }
    }

    /// <summary>
    /// Get the value of assigned buttons pressed
    /// </summary>
    /// <param name="ID">Purpose</param>
    /// <param name="button">The button to request</param>
    /// <returns>The value of the button.</returns>
    public float GetAxisRawValues(string ID, string button)
    {
        return !lockedInput && IDList[ID] ? axisRawValues[button] : 0;
    }

    /// <summary>
    /// Get the value of if the button is pressed.
    /// Usually lasts one frame.
    /// </summary>
    /// <param name="ID">Purpose</param>
    /// <param name="button">The button to request</param>
    /// <returns>The boolean value of the button getting pressed down.</returns>
    public bool GetButtonDown(string ID, string button)
    {
        return !lockedInput && IDList[ID] && buttonDowns[button];
    }

    /// <summary>
    /// Get the sate of the button.
    /// </summary>
    /// <param name="ID">Purpose</param>
    /// <param name="button">The button to request</param>
    /// <returns>The boolean value of the button state. True if it is pressed.</returns>
    public bool GetButtonStates(string ID, string button)
    {
        return !lockedInput && IDList[ID] && buttonStates[button];
    }
}
