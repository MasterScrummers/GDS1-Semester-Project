using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class SceneStartUp : MonoBehaviour
{
    [System.Serializable]
    private class Level
    {
        [field: SerializeField] public int numberOfRooms { get; private set; }
        [field: SerializeField] public RoomContent[] mandatoryContent { get; private set; }
    }

    private GameObject player;
    private Dictionary<string, GameObject> children;
    [SerializeField] private bool generateLevel = true;
    [SerializeField] private Level[] levels;

    void Start()
    {
        player = DoStatic.GetPlayer();

        children = new Dictionary<string, GameObject>();
        foreach(Transform child in DoStatic.GetChildren(transform))
        {
            children.Add(child.name, child.gameObject);
        }

        if (generateLevel)
        {
            VariableController vars = DoStatic.GetGameController<VariableController>();
            Level level = levels[vars.level];
            GetComponent<MapGenerator>().GenerateLevel(level.numberOfRooms, level.mandatoryContent);
            vars.IncrementLevel(levels.Length);
        }

        StartUp();
    }

    private void StartUp()
    {
        void TutorialStartUp()
        {
            Vector2 startPos = new(0, 5.5f); //Hardcoded, not to my liking...
            player.transform.position = startPos;
        }

        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.transform.eulerAngles = Vector3.zero;
        switch(DoStatic.GetSceneName())
        {
            case "Tutorial":
                TutorialStartUp();
                return;

            default:
                player.transform.position = new();
                return;
        }
    }
}
