using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteOutliner : MonoBehaviour
{
    private SpriteRenderer sprite;
    private SpriteRenderer outline;
    
    [SerializeField] private float outlineSizeMultiplier = 1.1f;
    [SerializeField] private Material mat;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        GameObject child = new GameObject(name + " outline");
        child.transform.parent = transform;
        child.transform.localPosition = Vector2.zero;
        child.transform.localScale *= outlineSizeMultiplier;

        outline = child.AddComponent<SpriteRenderer>();
        outline.sortingOrder--;
        outline.material = mat;
    }

    public void SetColour(Color newColor)
    {
        outline.color = newColor;
    }

    public void SetColour(Color32 newColor)
    {
        outline.color = newColor;
    }

    void Update()
    {
        outline.sprite = sprite.sprite;
    }
}
