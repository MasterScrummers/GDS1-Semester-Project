using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class UIEnemyHealthTracker : MonoBehaviour
{
    [SerializeField] private HealthComponent hp;
    private Image image;

    void Start()
    {
        image = GetComponent<Image>();
    }

    void Update()
    {
        if (hp)
        {
            image.fillAmount = hp.GetPercentage();
        }
    }
}
