#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class Lerper
{
    [SerializeField] private float start; //The start value
    public float currentValue { get; protected set; } //The current value depending on the current time between start and end value.
    [SerializeField] private float end; //The destination value
    private Timer timer; //The timer
    
    public bool isLerping { get; protected set; } = false; //A flag used to check if this class is still lerping.

    public void SetValues(float startValue, float endValue, float time, bool startLerping = true)
    {
        start = startValue;
        currentValue = start;
        end = endValue;

        timer = new Timer(time, 1);
        isLerping = startLerping;
    }

    public void Update(float deltaTime)
    {
        if (!isLerping)
        {
            return;
        }

        timer.Update(deltaTime);
        float clamp = Mathf.Clamp(timer.tick / timer.timer, 0, 1);
        currentValue = clamp * (end - start) + start;
        if (clamp == 1)
        {
            Reset();
        }
    }

    private void Reset()
    {
        timer.Reset();
        isLerping = false;
    }
}
