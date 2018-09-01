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
    public float zoomSpeed = 20;

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
            useMouseInput = false // anderer Skript
            Cursor.lockState = CursorLockMode.Locked;  // Testen
            Cursor.lockState = CursorLockMode.None;    // Testen
            transform.RotateAround(target.position, transform.right, -Input.GetAxis("Mouse Y") * freeCamRotationSpeed);
            transform.RotateAround(target.position, transform.up, -Input.GetAxis("Mouse X") * freeCamRotationSpeed);

             if (Input.GetKey(KeyCode.Escape))
                Screen.lockCursor = false;
             else
                Screen.lockCursor = true;

        }
        else
        {
            useMouseInput = true
            UpdateCamera();
        }
        */
    }

    private void UpdateCamera()
    {
        if (target != null)
        {
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                transform.position += this.transform.forward * scroll * zoomSpeed;
            
            transform.position = target.TransformPoint(startOffset);
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, rotateSpeed * Time.deltaTime);
        }
    }

}





