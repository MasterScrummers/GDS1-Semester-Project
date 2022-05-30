using UnityEngine;

public class VectorLerper
{
    private Lerper lerp;
    private Vector3 startVector; //The start
    private Vector3 endVector; //The end
    public Vector3 currentValue { get; private set; } //The current value.

    public bool isLerping { get; private set; } = false;

    public VectorLerper() {
        lerp = new Lerper();
    }

    public VectorLerper(Vector3 start, Vector3 end, float time, bool startLerping = true) : this()
    {
        SetValues(start, end, time, startLerping);
    }

    public void SetValues(Vector3 start, Vector3 end, float time, bool startLerping = true)
    {
        startVector = start;
        endVector = end;
        lerp.SetValues(0, 1, time, startLerping);
    }

    public void Update(float deltaTime)
    {
        lerp.Update(deltaTime);
        currentValue = Vector3.Lerp(startVector, endVector, lerp.currentValue);
        isLerping = lerp.isLerping;
    }
}