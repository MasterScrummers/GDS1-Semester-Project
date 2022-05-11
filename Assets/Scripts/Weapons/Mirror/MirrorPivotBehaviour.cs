using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MirrorPivotBehaviour : MonoBehaviour
{
    private float lifeTime;
    [SerializeField] private float originalLifeTime = 3f;

    private void Start()
    {
        OnEnable();
    }
    // Update is called once per frame
    void Update()
    {
        lifeTime -= Time.deltaTime;
        gameObject.SetActive(lifeTime > 0);
    }

    private void OnEnable()
    {
        lifeTime = originalLifeTime;
        foreach (Transform child in DoStatic.GetChildren(transform))
        {
            child.gameObject.SetActive(true);
        }

    }
}
