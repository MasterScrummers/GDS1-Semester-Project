using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public abstract class UISystemBase : MonoBehaviour
{
    protected InputController ic; //The input controller
    protected UIController UIc; //The UI controller

    protected CanvasGroup group; //The Canvas Group to toggle its visibility.
    private Lerper lerp; //To make the toggling of visibility smooth.

    private bool isActive; //Is the UI System active?
    private bool prevActive; //Allows StartUpdate() to run once.
    private bool hasStartedUp = false; //Has StartUp been called yet?

    private DoStatic.SimpleDelegate notification;

    /// <summary>
    /// Should be called through the UI Controller once.
    /// Acts like Start()
    /// </summary>
    public virtual void StartUp() {
        hasStartedUp = true;
        ic = DoStatic.GetGameController<InputController>();
        UIc = ic.GetComponent<UIController>();
        lerp = new Lerper();
        group = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Transitions in the UI.
    /// </summary>
    /// <param name="notify">Calls the method once it has transitioned in.</param>
    public void TransitionIn(float transitionTime, DoStatic.SimpleDelegate notify)
    {
        lerp.SetValues(0, 1, transitionTime);
        notification = notify;
    }

    /// <summary>
    /// Transitions out the UI.
    /// </summary>
    /// <param name="notify">Calls the method once it has transitioned out.</param>
    public void TransitionOut(float transitionTime)
    {
        lerp.SetValues(1, 0, transitionTime);
    }

    /// <summary>
    /// ALL CLASSES THAT INHERIT FROM THIS CLASS SHOULD NOT HAVE AN UPDATE FUNCTION.
    /// </summary>
    void Update()
    {
        if (!group)
        {
            return;
        }
        
        if (lerp.isLerping)
        {
            lerp.Update(Time.deltaTime);
            ic.SetInputLock(lerp.isLerping);
        }

        group.alpha = lerp.currentValue;
        isActive = group.alpha != 0;
        gameObject.SetActive(group.alpha != 0);

        if (!isActive)
        {
            return;
        }

        if (isActive != prevActive)
        {
            prevActive = isActive;
            FirstActiveFrameUpdate();
        }

        DoUpdate();
    }

    /// <summary>
    /// Meant to be overridden. Acts like the Updatefunction for all children classes.
    /// </summary>
    protected virtual void DoUpdate() {}

    /// <summary>
    /// NOT meant to be overridden. Look at DisableOverride.
    /// </summary>
    protected void OnDisable()
    {
        if (hasStartedUp)
        {
            group.alpha = 0;
            prevActive = false;
            DisableOverride();
        }
    }

    /// <summary>
    /// Meant to be overridden. For OnDisable().
    /// </summary>
    protected virtual void DisableOverride() {}

    /// <summary>
    /// Meant to be overridden.
    /// </summary>
    protected virtual void FirstActiveFrameUpdate() {
        if (notification != null)
        {
            notification();
        }
    }
}
