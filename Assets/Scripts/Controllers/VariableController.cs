#pragma warning disable IDE1006 // Naming Styles
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

    [System.Serializable]
    private class ColourPedia
    {
        [field: SerializeField] public string colourName { get; private set; }
        [field: SerializeField] public Color colour { get; private set; }
    }

    [System.Serializable]
    public class RoomColourPalettes
    {
        [field: SerializeField] public string colourName { get; private set; }
        [field: SerializeField] public Color backgroundColour { get; private set; }
        [field: SerializeField] public Color platformColour { get; private set; }
        [field: SerializeField] public Color doorColour { get; private set; }
        [field: SerializeField] public Color wallColour { get; private set; }
    }

    [SerializeField] private IconPedia[] icons;
    [SerializeField] private ColourPedia[] colours;
    [SerializeField] private RoomColourPalettes[] roomPalettes;
    private Dictionary<string, Sprite> allIcons;

    [SerializeField] private List<RoomContent> roomContents;

    private Dictionary<string, Color32> globalColours;
    private SceneController.SceneName scene = SceneController.SceneName.MainGame;
    public int level { get; private set; } = 0;
    public bool finalLevel { get; private set; } = false;

    public void IncrementLevel(int numberOfLevels)
    {
        level++;
        finalLevel = level == numberOfLevels;
    }

    public void ResetLevel()
    {
        level = 0;
    }

    void Awake()
    {
        allIcons = new();
        foreach (IconPedia icon in icons)
        {
            icon.AddIntoDictionary(ref allIcons);
        }

        globalColours = new();
        foreach (ColourPedia colour in colours)
        {
            globalColours.Add(colour.colourName, colour.colour);
        }
    }

    public Sprite GetIcon(string name)
    {
        return allIcons[name];
    }

    public Color32 GetColour(string colour)
    {
        return globalColours[colour];
    }

    public RoomColourPalettes GetRandomColourPalette()
    {
        return roomPalettes[Random.Range(0, roomPalettes.Length)];
    }

    public void SetScene(SceneController.SceneName scene)
    {
        this.scene = scene;
    }

    public SceneController.SceneName GetScene()
    {
        return scene;
    }

    public RoomContent GetRandomRoomContent()
    {
        return Instantiate(roomContents[Random.Range(0, roomContents.Count)]);
    }
}
