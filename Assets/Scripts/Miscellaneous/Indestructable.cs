using UnityEngine;

public class Indestructable : MonoBehaviour
{
    public bool onlyOneAllowed = true;

    void Awake()
    {
        if (onlyOneAllowed && GameObject.FindGameObjectsWithTag(gameObject.tag).Length > 1)
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(this);
    }
}
