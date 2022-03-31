using UnityEngine;

public class Rotate : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0, 0, 1); //1 is a revolution in the axis per second.

    void Update()
    {
        transform.Rotate(rotationSpeed * 360 * Time.deltaTime);
    }
}
