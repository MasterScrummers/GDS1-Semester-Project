using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 0.5f); //1 is a revolution in the axis per second.

    protected virtual void Update()
    {
        transform.Rotate(rotationSpeed * 360 * Time.deltaTime);
    }
}
