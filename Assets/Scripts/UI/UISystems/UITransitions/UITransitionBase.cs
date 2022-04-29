using System;
using UnityEngine;

public abstract class UITransitionBase : MonoBehaviour
{
    private Lerper transitionLerper; //A simple lerper to help with smooth transitions.
    private DoStatic.SimpleDelegate notification; //To call once transitioned in.
    public bool isReady { get; private set; } = false;

    /// <summary>
    /// Meant to be overridden.
    /// Acts like Start()
    /// </summary>
    public virtual void Initiate()
    {
        transitionLerper = new Lerper();
    }

    public void Activate(float transitionTime, DoStatic.SimpleDelegate notify)
    {
        gameObject.SetActive(true);
        Tivate(0, 1, transitionTime, notify);
    }
    public void Deactivate(float transitionTime, DoStatic.SimpleDelegate notify)
    {
        Tivate(1, 0, transitionTime, notify);
    }

    private void Tivate(float start, float end, float transitionTime, DoStatic.SimpleDelegate notify)
    {
        transitionLerper.SetValues(start, end, transitionTime);
        isReady = false;
        notification = notify;
    }

    protected void Update()
    {
        transitionLerper.Update(Time.deltaTime);
        UpdateDisplay(transitionLerper.currentValue);
        isReady = !transitionLerper.isLerping;
        if (isReady && notification != null)
        {
            notification();
            notification = null;
        }
    }

    protected virtual void UpdateDisplay(float transitionValue) {}

}
