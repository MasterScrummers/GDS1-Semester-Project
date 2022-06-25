using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashImagePool : MonoBehaviour
{
    [SerializeField] GameObject dashImagePrefab;

    private Queue<GameObject> availableObjects = new Queue<GameObject>(); //FIFO

    public static DashImagePool Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        GrowPool();
    }

    /// <summary>
    /// Instantiate 10 DashImagePrefab, add them to the Queue (availableObjects)
    /// </summary>
    private void GrowPool()
    {
        for (int i = 0; i < 10; i ++)
        {
            var instanceToAdd = Instantiate(dashImagePrefab);
            instanceToAdd.transform.SetParent(transform);
            AddToPool(instanceToAdd);
        }
    }

    /// <summary>
    /// Add the instance (DashImagePrefab) to the Queue(availableObjects), Set it to not active.
    /// </summary>
    /// <param name="instance"></param>
    public void AddToPool(GameObject instance)
    {
        instance.SetActive(false);
        availableObjects.Enqueue(instance);
    }

    /// <summary>
    /// If the Queue is empty, call GrowPool, otherwise Get the instance(DashImagePrefab) from the Queue, Set it to true, then Return it
    /// </summary>
    /// <returns>instance(DashImagePrefab)</returns>
    public GameObject GetFromPool()
    {
        if(availableObjects.Count == 0)
        {
            GrowPool();
        }

        var instance = availableObjects.Dequeue();
        instance.SetActive(true);
        return instance;
    }
}
