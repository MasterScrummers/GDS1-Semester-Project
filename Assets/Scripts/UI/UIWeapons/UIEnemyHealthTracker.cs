using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIEnemyHealthTracker : MonoBehaviour
{
    [SerializeField] private HealthComponent hp;
    private Image image;
    private VariableController vc;

    void Start()
    {
        image = GetComponent<Image>();
        vc = DoStatic.GetGameController<VariableController>();
    }

    void Update()
    {
        float percent = hp.GetPercentage();
        image.fillAmount = percent;
    }
}
