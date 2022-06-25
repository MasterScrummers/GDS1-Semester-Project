using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class SpriteFadeIn : MonoBehaviour
{
    private SpriteRenderer sprite;
    public Lerper alpha { get; private set; } = new();

    [SerializeField] private float fadeSpeed = 1f;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        alpha.SetValues(0, 1, fadeSpeed);
    }

    void Update()
    {
        if (!alpha.isLerping)
        {
            return;
        }

        alpha.Update(Time.deltaTime);
        SetAlpha(alpha.currentValue);
    }

    void OnEnable()
    {
        SetAlpha(0);
        alpha.SetValues(0, 1, fadeSpeed);
    }

    private void SetAlpha(float alpha)
    {
        Color col = sprite.color;
        col.a = alpha;
        sprite.color = col;
    }
}
