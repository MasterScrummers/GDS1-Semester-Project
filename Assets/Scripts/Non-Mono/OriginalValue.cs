#pragma warning disable IDE1006 // Naming Styles
using UnityEngine;

[System.Serializable]
public class OriginalValue<T>
{
    [field: SerializeField] public T originalValue { get; private set; }
    [HideInInspector] public T value;

    public OriginalValue(T originalValue)
    {
        SetOriginalValue(originalValue);
    }

    public void SetOriginalValue(T newValue)
    {
        originalValue = newValue;
        Reset();
    }

    public void Reset()
    {
        value = originalValue;
    }
}
