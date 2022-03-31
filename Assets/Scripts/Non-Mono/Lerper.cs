using UnityEngine;

public class Lerper
{
    private float start; //The start value
    public float currentValue; //The current value depending on the current time between start and end value.
    private float end; //The destination value

    private float currentTime; //The current time
    private float timeLimit; //The destination valye of current time
    
    public bool isLerping { get; protected set; } = false; //A flag used to check if this class is still lerping.

    public void SetValues(float startValue, float endValue, float time, bool startLerping = true)
    {
        start = startValue;
        currentValue = start;
        end = endValue;

        currentTime = 0;
        timeLimit = time;
        isLerping = startLerping;
    }

    public void Update(float deltaTime)
    {
        if (!isLerping)
        {
            return;
        }

        currentTime += deltaTime;
        float clamp = Mathf.Clamp(currentTime / timeLimit, 0, 1);
        currentValue = clamp * (end - start) + start;
        if (clamp == 1)
        {
            Reset();
        }
    }

    private void Reset()
    {
        currentTime = 0;
        isLerping = false;
    }
}
