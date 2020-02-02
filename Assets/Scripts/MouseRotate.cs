using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseRotate : MonoBehaviour
{
    [SerializeField] float rotationSpeed = 0.2f;

    void OnMouseDrag()
    {
        Debug.Log("fd");
        float XaxisRotation = Input.GetAxis("Mouse X") * rotationSpeed;
        float YaxisRotation = Input.GetAxis("Mouse Y") * rotationSpeed;
        transform.RotateAround(Vector3.down, XaxisRotation);
        transform.Rotate(Vector3.down, XaxisRotation);

        transform.RotateAround(Vector3.right, YaxisRotation);
    }
}