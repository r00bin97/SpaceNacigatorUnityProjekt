using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // <<<<<< ADD THIS.

public class RotationMainMenu : MonoBehaviour {
    [SerializeField]
    public float rotationSpeed = 15f;

    public void Update ()
    {
        transform.Rotate(Vector3.up * Time.deltaTime * rotationSpeed);

        if (Input.GetMouseButton(0))
          SceneManager.LoadScene("d");
      //  EventManager.StartGame();

    }
}
