using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class RainbowCube : MonoBehaviour
{
    private SpriteRenderer sprite;
    [SerializeField] private float colourSpeed = 5;

    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        sprite.color = ShiftHueBy(sprite.color, colourSpeed * Time.deltaTime);
    }

    private Color ShiftHueBy(Color color, float amount)
    {
        // convert from RGB to HSV
        Color.RGBToHSV(color, out float hue, out float sat, out float val);

        // shift hue by amount
        hue += amount;

        // convert back to RGB and return the color
        return Color.HSVToRGB(hue, sat, val);
    }
}
