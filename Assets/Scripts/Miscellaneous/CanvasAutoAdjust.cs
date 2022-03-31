using UnityEngine;
using UnityEngine.UI;

public class CanvasAutoAdjust : MonoBehaviour
{
    void Start()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.pixelPerfect = true;
        canvas.worldCamera = DoStatic.GetGameController().GetComponentInChildren<Camera>();
        canvas.sortingLayerName = gameObject.tag.Equals("Transition") ? "Transition" : canvas.sortingLayerName;

        CanvasScaler scaler = GetComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        Destroy(this);
    }
}
