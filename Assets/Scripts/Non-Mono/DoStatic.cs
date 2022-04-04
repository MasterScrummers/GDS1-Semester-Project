using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoStatic
{
    /// <summary>
    /// Get the name of the current scene.
    /// </summary>
    /// <returns>A string of the current scene.</returns>
    public static string GetSceneName()
    {
        return SceneManager.GetActiveScene().name;
    }

    /// <summary>
    /// Loads the scene (by string).
    /// </summary>
    /// <param name="sceneName">The scene name to load into.</param>
    /// <param name="asAdditive">Adds the loaded scene on top of current scene.</param>
    public static AsyncOperation LoadScene(string sceneName, bool asAdditive = true)
    {
        return SceneManager.LoadSceneAsync(sceneName, asAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    /// <summary>
    /// Loads the scene (by index).
    /// </summary>
    /// <param name="index">Scene to load index.</param>
    /// <param name="asAdditive">Adds the loaded scene on top of current scene.</param>
    public static AsyncOperation LoadScene(int index, bool asAdditive = true)
    {
        return LoadScene(SceneManager.GetSceneByBuildIndex(index).name, asAdditive);
    }

    /// <summary>
    /// Moves a gameobject to another scene.
    /// </summary>
    /// <param name="obj">Gameobject to move.</param>
    /// <param name="scene">Scene to move the GameObject into.</param>
    public static void MoveGameObjectToScene(GameObject obj, Scene scene)
    {
        SceneManager.MoveGameObjectToScene(obj, scene);
    }

    /// <summary>
    /// Unload a given scene (by string).
    /// </summary>
    /// <param name="sceneName">The scene to unload.</param>
    /// <returns>An AsyncOperation to know when the scene has fully unloaded.</returns>
    public static AsyncOperation UnloadScene(string sceneName)
    {
        return SceneManager.UnloadSceneAsync(sceneName);
    }

    /// <summary>
    /// Get a random colour.
    /// </summary>
    /// <returns>A random colour.</returns>
    public static Color RandomColor()
    {
        return new Color(Random.value, Random.value, Random.value);
    }

    /// <summary>
    /// Get a random bool.
    /// </summary>
    /// <param name="successRate">The successRate. Defaults to 50%. Should be between 0 to 1</param>
    /// <returns>A random bool.</returns>
    public static bool RandomBool(float successRate = 0.5f)
    {
        return Random.value > 1 - successRate;
    }

    /// <summary>
    /// Gets all the children in the given gameobject.
    /// This is a recursive function with utilising the Depth First Search algorithm.
    /// </summary>
    /// <param name="transform">The transform of the gameobject.</param>
    /// <param name="generationDepth">The depth of the search.</param>
    /// <param name="childrenRef">Starting children, if any, mainly used for generation depth.</param>
    /// <returns>An array of all the children.</returns>
    public static Transform[] GetChildren(Transform transform, int generationDepth = 1, List<Transform> childrenRef = null)
    {
        generationDepth--;
        List<Transform> children = childrenRef != null ? childrenRef : new List<Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            children.Add(child);
            if (generationDepth > 0 && child.childCount > 0)
            {
                GetChildren(child, generationDepth, children);
            }
        }
        return children.ToArray();
    }

    private static GameObject GetObject(string tag)
    {
        return GameObject.FindGameObjectWithTag(tag);
    }

    /// <summary>
    /// Finds the GameObject with the tag "GameController" and grabs the component specified.
    /// There should only be one!
    /// You can access to the GameObject after grabbing the Transform component.
    /// </summary>
    /// <returns>The GameController GameObject</returns>
    public static T GetGameController<T>()
    {
        return GetObject("GameController").GetComponent<T>();
    }

    /// <summary>
    /// Finds the GameObject with the tag "Player".
    /// There should only be one!
    /// </summary>
    /// <returns>The Player GameObject</returns>
    public static GameObject GetPlayer()
    {
        return GetObject("Player");
    }
}
