using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Image))]
public abstract class UICircleBarBase : MonoBehaviour
{
    protected Image circle; //The circle bar itself.

    protected virtual void Start()
    {
        circle = GetComponent<Image>();
        circle.type = Image.Type.Filled;
        circle.fillMethod = Image.FillMethod.Radial360;
        circle.fillOrigin = 2;
        circle.fillAmount = 1;
    }
}
