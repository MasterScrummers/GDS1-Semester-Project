using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class CollisionDisplay : MonoBehaviour
{
    [SerializeField] private GameObject display; //The tutorial indicator.

    void Start()
    {
        GetComponent<Collider2D>().isTrigger = true;
        display.SetActive(false);
    }

    private void PlayerCheck(Collider2D collision, bool active)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            display.SetActive(active);
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        PlayerCheck(collision, true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        PlayerCheck(collision, false);
    }
}
