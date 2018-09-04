using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationMainMenu : MonoBehaviour {
    [SerializeField]
    public float rotationSpeed = 15f;

    public void Update ()
    {
        
        
            transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);
        
    }
}
