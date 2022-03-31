using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Aims to keep track, save and load all important variables across all save files.
/// </summary>
public class SaveController : MonoBehaviour
{
    private BinaryFormatter formatter; //Converts data from and into a serialised format.

    private string saveFilePath = "High Score"; //The path of where the saved file is at.
    private Dictionary<string, object> savedVars; //The dictionary of where all the data is saved.

    void Start()
    {
        formatter = new BinaryFormatter();
        savedVars = new Dictionary<string, object>();
        SetFilename(saveFilePath);
    }

    /// <summary>
    /// Sets the save file's name.
    /// </summary>
    /// <param name="filename">Name of the save file.</param>
    public void SetFilename(string filename)
    {
        saveFilePath = Application.persistentDataPath + "/" + filename + ".sav";
    }

    /// <summary>
    /// Saves the current state of the dictionary.
    /// </summary>
    public void Save()
    {
        FileStream file = File.Create(saveFilePath);
        formatter.Serialize(file, savedVars);
        file.Close();
    }

    private Dictionary<string, object> LoadFile(string filename)
    {
        if (!File.Exists(filename))
        {
            Debug.Log("Unknown Save data!");
            return new Dictionary<string, object>();
        }

        savedVars.Clear();
        FileStream file = File.Open(filename, FileMode.Open);
        Dictionary<string, object> vars;
        vars = (Dictionary<string, object>)formatter.Deserialize(file);
        file.Close();
        return vars;
    }

    /// <summary>
    /// Load the variables from the file.
    /// </summary>
    public void Load()
    {
        savedVars = LoadFile(saveFilePath);
    }

    /// <summary>
    /// Loads variables from file but does not remember them.
    /// Useful to grab some variables from the file.
    /// </summary>
    /// <param name="filename">The path to and name of the file.</param>
    /// <returns>A dictionary of all the varibles in the files.</returns>
    public Dictionary<string, object> LiteLoad(string filename)
    {
        return LoadFile(filename);
    }

    /// <summary>
    /// Saves the variable called through from other components.
    /// Warning: If the object already exists, it will be overwritten.
    /// </summary>
    /// <param name="key">The string of the variable name, will error if there is more than one key.</param>
    /// <param name="obj">Any variable that can be serialised.</param>
    public void SaveVariable(string key, object obj)
    {
        if (savedVars.ContainsKey(key))
        {
            savedVars[key] = obj;
        } else
        {
            savedVars.Add(key, obj);
        }
    }

    /// <summary>
    /// Loads the variable needed. Casting will be needed for the variable.
    /// <br/><br/>Warning: Use LoadNumberVariable if variable wanted is an int or float
    /// because they cannot be null.
    /// </summary>
    /// <param name="key">The key for the wanted variable.</param>
    /// <returns>The object if the key exists otherwise returns null.</returns>
    public object LoadVariable(string key)
    {
        return savedVars.ContainsKey(key) ? savedVars[key] : null;
    }

    /// <summary>
    /// Loads the number variable needed.
    /// </summary>
    /// <param name="key">The key for the wanted number variable.</param>
    /// <returns>A float if the key exists otherwise returns 0.</returns>
    public float LoadNumberVariable(string key)
    {
        return (float)savedVars[key];
    }
}
