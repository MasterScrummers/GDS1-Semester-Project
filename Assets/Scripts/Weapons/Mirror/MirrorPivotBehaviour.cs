using UnityEngine;

public class MirrorPivotBehaviour : MonoBehaviour
{
    [SerializeField] private Timer lifeTime = new(3);

    private void Start()
    {
        OnEnable();
    }

    void Update()
    {
        lifeTime.Update(Time.deltaTime);
        gameObject.SetActive(lifeTime.tick == 0);
    }

    private void OnEnable()
    {
        lifeTime.Reset();
        foreach (Transform child in DoStatic.GetChildren(transform))
        {
            child.gameObject.SetActive(true);
        }

    }
}
