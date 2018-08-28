using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    public float rotateSpeed = 90.0f;
    private Transform target;
    private Vector3 startOffset;
 //   public float freeCamRotationSpeed = 30.0f;   // --> ToDo -> Add Free Rotation for Camera...
 
    private void Start()
    {
        // Hole target von Parent Object. 
        target = transform.parent;
        startOffset = transform.localPosition;
        transform.SetParent(null);
    }

    private void Update()
    {
        UpdateCamera();
        /* // Free Camera Rotation --> 
        if (Input.GetKey(KeyCode.P))
        {
            transform.RotateAround(target.position, transform.right, -Input.GetAxis("Mouse Y") * freeCamRotationSpeed);
            transform.RotateAround(target.position, transform.up, -Input.GetAxis("Mouse X") * freeCamRotationSpeed);
        }
        else
        {
            UpdateCamera();
        }
        */

    }

    private void UpdateCamera()
    {
        if (target != null)
        {
            transform.position = target.TransformPoint(startOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);
        }
    }

}
