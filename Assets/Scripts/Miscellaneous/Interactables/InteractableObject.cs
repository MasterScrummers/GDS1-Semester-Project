using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class InteractableObject : MonoBehaviour
{
    protected InputController ic; //Input controller to check inputs.
    protected Collider2D col;
    protected PlayerInput pi;
    protected bool nearPlayer = false;

    protected virtual void Awake()
    {
        col = GetComponent<Collider2D>();
        col.enabled = true;
        col.isTrigger = true;
        ic = DoStatic.GetGameController<InputController>();
        pi = DoStatic.GetPlayer<PlayerInput>();
    }

    protected virtual void Update()
    {
        if (nearPlayer && ic.GetButtonDown("Movement", "Interact"))
        {
            Interact();
        }
    }

    protected virtual void Interact()
    {
        col.enabled = false;
        nearPlayer = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nearPlayer = true;
            pi.canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            nearPlayer = false;
            pi.canInteract = ic.GetButtonDown("Movement", "Interact");
        }
    }
}
