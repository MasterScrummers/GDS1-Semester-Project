using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 1. TransitionIn
/// 2. TransitionOut
/// </summary>
[RequireComponent(typeof(Image))]
public class TransitionFade : TransitionBase
{
    protected Lerper lerp; //The lerper, not much to say here.
    private Image background; //The background that is transitioned into.

    protected override void Start()
    {
        transitionName = "Fade";
        lerp = new Lerper();
        background = GetComponent<Image>();
        base.Start();
    }

    public override void InitiateTransition()
    {
        AssignState(TransitionIn);
    }

    protected virtual void TransitionIn(bool isStartUp)
    {
        if (isStartUp)
        {
            lerp.SetValues(0, 1, timeIn);
            return;
        }

        background.color = UpdateAlpha(background.color);

        if (GetType().Equals(typeof(TransitionFade)) && !lerp.isLerping)
        {
            startLoading = true;
            if (startFinishingUp)
            {
                AssignState(TransitionOut);
            }
        }
    }

    protected void TransitionOut(bool isStartUp)
    {
        if (isStartUp)
        {
            lerp.SetValues(1, 0, timeOut);
            return;
        }

        background.color = UpdateAlpha(background.color);

        if (!lerp.isLerping)
        {
            AssignState(null);
        }
    }

    protected Color UpdateAlpha(Color currentColour)
    {
        lerp.Update(Time.deltaTime);
        return new Color(currentColour.r, currentColour.g, currentColour.b, lerp.currentValue);
    }
}
