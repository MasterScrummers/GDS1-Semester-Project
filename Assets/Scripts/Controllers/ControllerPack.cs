using UnityEngine;

[RequireComponent(typeof(Indestructable))]
[RequireComponent(typeof(VariableController))]
[RequireComponent(typeof(AudioController))]
[RequireComponent(typeof(SceneController))]
[RequireComponent(typeof(InputController))]
[RequireComponent(typeof(SaveController))]
[RequireComponent(typeof(UIController))]
public class ControllerPack : MonoBehaviour
{
    /* This script does nothing but ensures all the
     * controllers are on the attached gameobject.
     * 
     * It's also a lazy method, just add this script
     * as a component and Unity will add all of the
     * required components.
     */
}
