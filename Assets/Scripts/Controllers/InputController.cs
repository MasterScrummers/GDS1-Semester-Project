#pragma warning disable IDE1006 // Naming Styles
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public bool isInputLocked { get; private set; } = false; //Bool to (un)lock all inputs.
    private Dictionary<string, bool> inputReasons; //Allows inputs depending on the boolean set. Has to be part of IDList to work.

    private Dictionary<string, float> axisRawValues; //All inputs that checks Input.GetAxisRaw.
    private Dictionary<string, bool> buttonDowns; //All inputs that checks Input.GetButtonDown.
    private Dictionary<string, bool> buttonStates; //All inputs that checks Input.GetButton.

    private delegate Value InputCheck<Value>(string key); //Delegate variable for dynamic dictionary updates reusing code.

    private void Awake()
    {
        //Initiation + Default values
        inputReasons = new Dictionary<string, bool>();
        foreach(string input in new string[] { //Treat the ID list like they are GameObject tags
            "Attack", //Related to attack inputs.
            "Movement", //Related to movement
            "WeaponSwap", //The weapon swap system
            "MenuNavigation", //To navigate through the main menu.
            "Credits", //To exit the credits.
        })
        {
            inputReasons.Add(input, true);
        }

        //Add the inputs in the correct dictionary here!
        axisRawValues = new Dictionary<string, float>();
        buttonDowns = new Dictionary<string, bool>();
        buttonStates = new Dictionary<string, bool>();
        foreach (string input in new string[]
        {
            "Horizontal", //[D, A, left, right]
            "Vertical", //[W, S, up, down]

            //More for keydown.
            "Jump", //[space, w]
            "Light", //j
            "Heavy", //k
            "Special", //l
            "Interact", //w
            "Exit", //escape
        }) {
            axisRawValues.Add(input, GetAxisRaw(input));
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
    public void SetInputLock(bool isLocked)
    {
        isInputLocked = isLocked;
    }

    /// <summary>
    /// Adds a reason to the blacklist.
    /// Makes InputController return negative for that reason.
    /// </summary>
    /// <param name="ID">The reason</param>
    public void SetInputReason(string ID, bool state)
    {
        if (inputReasons.ContainsKey(ID))
        {
            inputReasons[ID] = state;
        }
    }

    /// <summary>
    /// Get the state of an ID.
    /// </summary>
    /// <param name="reason">The reason to check.</param>
    /// <returns>Will always return false if the ID does not exist.</returns>
    public bool GetInputReason(string reason)
    {
        return inputReasons[reason];
    }

    /// <summary>
    /// Get the value of assigned buttons pressed
    /// </summary>
    /// <param name="reason">Purpose</param>
    /// <param name="button">The button to request</param>
    /// <returns>The value of the button.</returns>
    public float GetAxisRawValues(string reason, string button)
    {
        return !isInputLocked && inputReasons[reason] ? axisRawValues[button] : 0;
    }

    /// <summary>
    /// Get the value of if the button is pressed.
    /// Lasts one frame.
    /// </summary>
    /// <param name="reason">Purpose</param>
    /// <param name="button">The button to request</param>
    /// <returns>The boolean value of the button getting pressed down.</returns>
    public bool GetButtonDown(string reason, string button)
    {
        return !isInputLocked && inputReasons[reason] && buttonDowns[button];
    }

    /// <summary>
    /// Get the sate of the button.
    /// </summary>
    /// <param name="reason">Purpose</param>
    /// <param name="button">The button to request</param>
    /// <returns>The boolean value of the button state. True if it is pressed.</returns>
    public bool GetButtonStates(string reason, string button)
    {
        return !isInputLocked && inputReasons[reason] && buttonStates[button];
    }
}
