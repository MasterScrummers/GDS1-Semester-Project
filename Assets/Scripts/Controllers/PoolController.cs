using System.Collections.Generic;
using UnityEngine;

public class PoolController : MonoBehaviour
{
    [SerializeField] private GameObject poolManager;
    private Dictionary<string, ObjectPool> pools;

    void Start()
    {
        pools = new();
        foreach (Transform child in DoStatic.GetChildren(poolManager.transform))
        {
            ObjectPool pool = child.GetComponent<ObjectPool>();
            if (pool)
            {
                pools.Add(pool.name, pool);
                continue;
            }
            Debug.Log("GameObject " + pool.name + " does not have a ObjectPool component.");
        }
    }

    /// <summary>
    /// Get a GameObject from the specified pool.
    /// </summary>
    /// <param name="poolName">The pool to the GameObject from.</param>
    /// <param name="autoActive">Activate given GameObject or not? Defaults to true.</param>
    /// <returns>The GameObject from the specified pool.</returns>
    public GameObject GetObjectFromPool(string poolName, bool autoActive = true)
    {
        return pools[poolName].GetObject(autoActive);
    }
}
