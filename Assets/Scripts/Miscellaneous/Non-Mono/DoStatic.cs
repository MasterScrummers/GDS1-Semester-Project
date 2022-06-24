using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoStatic
{
    public delegate void SimpleDelegate();

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
    public static AsyncOperation LoadScene(string sceneName, bool asAdditive = false)
    {
        return SceneManager.LoadSceneAsync(sceneName, asAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single);
    }

    /// <summary>
    /// Loads the scene (by index).
    /// </summary>
    /// <param name="index">Scene to load index.</param>
    /// <param name="asAdditive">Adds the loaded scene on top of current scene.</param>
    public static AsyncOperation LoadScene(int index, bool asAdditive = false)
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
        List<Transform> children = childrenRef ?? new List<Transform>();
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
    /// Get a child with tag. Utilises GetChildren() meaning, it is a recursive function.
    /// </summary>
    /// <param name="tag">The first tag to find</param>
    /// <param name="parent">The parent to search through.</param>
    /// <param name="generationDepth">The depth of the search.</param>
    /// <returns></returns>
    public static GameObject GetChildWithTag(string tag, Transform parent, int generationDepth = 1)
    {
        foreach (Transform child in GetChildren(parent, generationDepth))
        {
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
        }
        return null;
    }

    /// <summary>
    /// Finds the GameObject with the tag "GameController".
    /// There should only be one!
    /// </summary>
    /// <returns>The GameController GameObject</returns>
    public static GameObject GetGameController()
    {
        return GetObject("GameController");
    }

    /// <summary>
    /// Finds the GameObject with the tag "GameController" and grabs the component specified.
    /// There should only be one!
    /// </summary>
    /// <returns>The GameController GameObject</returns>
    public static T GetGameController<T>() where T : Component
    {
        return GetGameController().GetComponent<T>();
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

    /// <summary>
    /// Finds the GameObject with the tag "Player" and grabs the component specified.
    /// There should only be one!
    /// </summary>
    /// <returns>The Component from the player Gameobject</returns>
    public static T GetPlayer<T>() where T : Component
    {
        return GetPlayer().GetComponentInChildren<T>();
    }

    /// <summary>
    /// Swaps the values between two variables.
    /// </summary>
    /// <param name="a">The first variable</param>
    /// <param name="b">The second variable</param>
    public static void Swap<T>(ref T a, ref T b)
    {
        (b, a) = (a, b);
    }

    /// <summary>
    /// Get the rotation to look at.
    /// </summary>
    /// <param name="from">The starting position to look</param>
    /// <param name="target">The target position to look</param>
    public static void LookAt(Transform from, Vector3 target)
    {
        from.right = target - from.position;
    }

    /// <summary>
    /// Shuffles the given array.
    /// </summary>
    /// <typeparam name="T">Any dayatype</typeparam>
    /// <param name="arr">An array.</param>
    public static void ShuffleArray<T>(T[] arr)
    {
        for (int element = 0; element < arr.Length; element++)
        {
            Swap(ref arr[element], ref arr[Random.Range(0, arr.Length)]);
        }
    }
}
