using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 1. TransitionIn (parent, override)
/// 2. WaitingIn
///     2.1 LoadingIn
///     2.2 LoadingOut
/// 5. TransitionOut (parent)
/// </summary>
public class TransitionFadeWithLogo : TransitionFade
{
    private Image logo; //The logo to rotate.
    public float timeBeforeLogo = 0.11f; //The time before the logo appears.

    protected override void Start()
    {
        foreach (Transform child in DoStatic.GetChildren(transform))
        {
            logo = child.GetComponent<Image>();
            if (logo)
            {
                logo.color = new Color(1, 1, 1, 0);
                break;
            }
        }
        base.Start();
    }

    protected override void TransitionIn(bool isStartUp)
    {
        base.TransitionIn(isStartUp);

        if (GetType().Equals(typeof(TransitionFadeWithLogo)) && !lerp.isLerping) {
            AssignState(WaitingIn); 
        }
    }

    protected virtual void WaitingIn(bool isStartUp)
    {
        if (isStartUp)
        {
            startLoading = true;
            lerp.SetValues(0, 1, timeBeforeLogo);
            return;
        }

        lerp.Update(Time.deltaTime);

        if (!lerp.isLerping)
        {
            AssignState(LoadingIn);
        }

        if (startFinishingUp)
        {
            AssignState(TransitionOut);
        }
    }

    protected virtual void LoadingIn(bool isStartUp)
    {
        if (isStartUp)
        {
            lerp.SetValues(0, 1, timeIn);
            return;
        }

        logo.color = UpdateAlpha(logo.color);

        if (!lerp.isLerping && startFinishingUp)
        {
            AssignState(LoadingOut);
        }
    }

    protected virtual void LoadingOut(bool isStartUp)
    {
        if (isStartUp)
        {
            lerp.SetValues(1, 0, timeOut);
            return;
        }

        logo.color = UpdateAlpha(logo.color);

        if (!lerp.isLerping)
        {
            AssignState(TransitionOut);
        }
    }
}
