using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MapGenerator))]
public class SceneStartUp : MonoBehaviour
{
    private GameObject player;
    private Dictionary<string, GameObject> children;
    [SerializeField] private bool generateLevel = true;

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
            GetComponent<MapGenerator>().GenerateLevel();
        }

        StartUp();
    }

    private void StartUp()
    {
        void TutorialStartUp()
        {
            Vector2 startPos = new Vector2(0, 5.5f); //Hardcoded, not to my liking...
            player.transform.position = startPos;
        }

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
