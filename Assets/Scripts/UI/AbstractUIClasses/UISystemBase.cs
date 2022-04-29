using UnityEngine;

public abstract class UISystemBase : MonoBehaviour
{
    [SerializeField] protected float transitionTime = 1; //Is in seconds

    protected InputController ic; //The input controller
    protected UIController UIc; //The UI controller

    protected CanvasGroup group; //The Canvas Group to toggle its visibility.
    private Lerper transitionLerp; //To make the toggling of visibility smooth.

    private bool isActive; //Is the UI System active?
    private bool prevActive; //Allows StartUpdate() to run once.

    private DoStatic.SimpleDelegate notification; //Simple notification that is called when first transitioning in.

    /// <summary>
    /// Should be called through the UI Controller once.
    /// Acts like Start()
    /// </summary>
    public virtual void Initiate() {
        ic = DoStatic.GetGameController<InputController>();
        UIc = ic.GetComponent<UIController>();
        transitionLerp = new Lerper();
        group = GetComponent<CanvasGroup>();
    }

    /// <summary>
    /// Transitions in the UI.
    /// </summary>
    /// <param name="notify">Calls the method once it has transitioned in.</param>
    public virtual void Activate(DoStatic.SimpleDelegate notify = null)
    {
        gameObject.SetActive(true);
        transitionLerp.SetValues(0, 1, transitionTime);
        notification = notify;
    }

    /// <summary>
    /// Transitions out the UI.
    /// </summary>
    /// <param name="notify">Calls the method once it has transitioned out.</param>
    public virtual void Deactivate(DoStatic.SimpleDelegate notify = null)
    {
        transitionLerp.SetValues(1, 0, transitionTime);
        notification = notify;
    }

    /// <summary>
    /// Call this if the GameObject has a CanvasGroup component in the Update procedure.
    /// </summary>
    /// <returns>True if it is still transitioning in/out.</returns>
    protected virtual bool DoTransitioning()
    {
        if (!group)
        {
            return false;
        }

        if (transitionLerp.isLerping)
        {
            transitionLerp.Update(Time.deltaTime);
            ic.SetInputLock(transitionLerp.isLerping);
        }

        group.alpha = transitionLerp.currentValue;
        isActive = group.alpha != 0;
        gameObject.SetActive(group.alpha != 0);

        if (!isActive)
        {
            return true;
        }

        if (isActive != prevActive)
        {
            prevActive = isActive;
            FirstActiveFrameUpdate();
        }

        return false;
    }

    /// <summary>
    /// Meant to be overridden.
    /// </summary>
    protected virtual void FirstActiveFrameUpdate()
    {
        Notify();
    }

    protected void Notify()
    {
        if (notification != null)
        {
            notification();
        }
    }

    protected virtual void OnDisable()
    {
        if (group)
        {
            group.alpha = 0;
        }
        prevActive = false;
    }
}
