using System.Collections.Generic;
using UnityEngine;

public class VariableController : MonoBehaviour
{
    [System.Serializable]
    private class IconPedia
    {
        [SerializeField] private string iconName;
        [SerializeField] private Sprite iconSprite;

        public void AddIntoDictionary(ref Dictionary<string, Sprite> allWeapons)
        {
            allWeapons.Add(iconName, iconSprite);
        }
    }

    [SerializeField] private IconPedia[] icons;
    private Dictionary<string, Sprite> allIcons;
    private Dictionary<string, Color32> globalColours;
    private SceneController.SceneName scene = SceneController.SceneName.MapTestScene;
    private int level = 1;

    public void SetLevel(int newLevel)
    {
        level = newLevel;
    }

    public int GetLevel()
    {
        return level;
    }

    public void OffsetLevel(int amount)
    {
        level += amount;
    }

    void Awake()
    {
        allIcons = new();
        foreach (IconPedia icon in icons)
        {
            icon.AddIntoDictionary(ref allIcons);
        }

        globalColours = new()
        {
            ["Rubik Red"] = new(183, 18, 52, 255),
            ["Rubik Green"] = new(0, 155, 72, 255),
            ["Rubik White"] = Color.white,
            ["Rubik Orange"] = new(255, 88, 0, 255),
            ["Rubik Blue"] = new(0, 70, 173, 255),
            ["Rubik Yellow"] = new(255, 213, 0, 255),

            ["Gray"] = Color.gray,
        };
    }

    public Sprite GetIcon(string name)
    {
        return allIcons[name];
    }

    public Color32 GetColour(string colour)
    {
        return globalColours[colour];
    }

    public void SetScene(SceneController.SceneName scene)
    {
        this.scene = scene;
    }

    public SceneController.SceneName GetScene()
    {
        return scene;
    }
}
