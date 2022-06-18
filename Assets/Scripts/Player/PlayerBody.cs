#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

public class PlayerBody : MonoBehaviour
{
    [field: SerializeField] public Collider2D col { get; private set; }
}
