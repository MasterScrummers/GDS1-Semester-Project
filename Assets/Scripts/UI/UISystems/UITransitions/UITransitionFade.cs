using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class UITransitionFade : UITransitionBase
{
    private CanvasGroup fade;

    public override void Initiate()
    {
        base.Initiate();
        fade = GetComponent<CanvasGroup>();
        fade.alpha = 0;
    }

    protected override void UpdateDisplay(float transitionValue)
    {
        fade.alpha = transitionValue;
    }
}
