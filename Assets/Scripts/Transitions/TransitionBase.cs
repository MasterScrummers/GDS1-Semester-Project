using UnityEngine;

public abstract class TransitionBase : MonoBehaviour
{
    public bool startLoading { get; protected set; } = false; //The signal for SceneController to start loading.
    public bool startFinishingUp = false; //Signals the start and end of the transition.
    public string transitionName { get; protected set; } //Name of the transition, used in SceneController.
    protected float timeIn; //Fade in speed.
    protected float timeOut; //Fade out speed.

    /// <summary>
    /// Simple delegate method that takes a bool. When assigning it, the method must be structured like so:
    /// <br/>...void example(bool isStartUp = false) {
    /// <br/>----if (isStartUp) {
    /// <br/>--------//Do stuff here.
    /// <br/>--------return;
    /// <br/>----} else {
    /// <br/>--------//Do stuff here.
    /// <br/>--------//When conditions satisfy, assign the next delegate and continue.
    /// <br/>--------//When finished, assign doTransition to null.
    /// <br/>----}
    /// <br/>}
    /// </summary>
    /// <param name="isStartUp">Defaults to false, when reassigning, true will be called.</param>
    protected delegate void DoTransition(bool isStartUp = false);
    private DoTransition doTransition = null;

    /// <summary>
    /// Call this last in the override.
    /// </summary>
    protected virtual void Start()
    {
        float[] times = DoStatic.GetGameController<SceneController>().GetTransitionTimes(this);
        timeIn = times[0];
        timeOut = times[1];
    }

    void Update()
    {
        if (doTransition != null)
        {
            doTransition();
        }
    }

    /// <summary>
    /// Start the transition.
    /// </summary>
    public virtual void InitiateTransition() {}

    /// <summary>
    /// Assign the next stage of the transition.
    /// </summary>
    /// <param name="transition">Must assign the next transition, null for the ending last transition state.</param>
    protected void AssignState(DoTransition transition)
    {
        doTransition = transition;
        if (doTransition != null)
        {
            doTransition(true);
        } else
        {
            startFinishingUp = false;
        }
    }
}
