using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [SerializeField] private GameObject prefab; //Has to be given in the editor.

    /// <summary>
    /// Get a GameObject in the pool.
    /// </summary>
    /// <param name="autoActivate">Activate the GameObject upon return? Defaults to true.</param>
    /// <returns>A GameObject from the pool.</returns>
    public GameObject GetObject(bool autoActivate = true)
    {
        GameObject obj;
        foreach (Transform child in DoStatic.GetChildren(transform))
        {
            obj = child.gameObject;
            if (!obj.activeInHierarchy)
            {
                obj.SetActive(autoActivate);
                return obj;
            }
        }
        obj = Instantiate(prefab, transform);
        obj.SetActive(autoActivate);
        return obj;
    }
}
