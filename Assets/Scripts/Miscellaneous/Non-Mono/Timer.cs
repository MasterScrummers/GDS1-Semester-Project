#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

[System.Serializable]
public class Timer
{
    [field: SerializeField] public float timer {get; private set;} //The time limit
    public float tick {get; private set;} //The current time left
    [field: SerializeField] public float timeScale { get; private set; } = 1; //Counts up if positive, counts down if negative. Defaults to countdown.

    /// <summary>
    /// A simple timer. Defaults as a countdown timer.
    /// </summary>
    /// <param name="timer">Set the timer where the tick will start at or go to</param>
    /// <param name="timeScale">Dictates the speed of the timer and it counts up (+) or down (-).</param>
    public Timer(float timer, float timeScale = -1)
    {
        SetTimeScale(timeScale);
        SetTimer(timer);
    }

    /// <summary>
    /// Set the time scale, how fast is this timer?
    /// </summary>
    /// <param name="timeScale">Dictates the speed of the timer and it counts up (+) or down (-).</param>
    public void SetTimeScale(float timeScale)
    {
        this.timeScale = timeScale;
    }

    /// <summary>
    /// Change the timer.
    /// </summary>
    /// <param name="timer">Timer value</param>
    /// <param name="resetTick">Auto call reset?</param>
    public void SetTimer(float timer, bool resetTick = true)
    {
        this.timer = timer;
        if (resetTick)
        {
            Reset();
        }
    }

    /// <summary>
    /// Resets the tick to 0 or the timer depending on the timescale.
    /// </summary>
    public void Reset()
    {
        tick = timeScale > 0 ? 0 : timer;
    }

    /// <summary>
    /// Finishs the timer, opposite of Reset().
    /// </summary>
    public void Finish()
    {
        tick = timeScale > 0 ? timer : 0;
    }

    /// <summary>
    /// Updates the timer.
    /// </summary>
    /// <param name="deltaTime">Pass through the UnityEngine.Time.deltaTime or another controlled time variable</param>
    public void Update(float deltaTime)
    {
        float delta = timeScale * deltaTime;
        tick = Mathf.Clamp(tick + delta, 0, timer);
    }
}
