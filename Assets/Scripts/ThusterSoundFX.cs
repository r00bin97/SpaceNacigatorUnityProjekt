using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThusterSoundFX : MonoBehaviour {

    AudioSource audioThruster;
    // Use this for initialization
    void Start()
    {
        audioThruster = GetComponent<AudioSource>();
        audioThruster.Pause();
    }

    // Update is called once per frame
    void Update () {

        // Verbraucht Tank bei Tastendruck W
        if (Input.GetKey(KeyCode.W))
        {
            audioThruster.UnPause();
        }
        else
        {
            audioThruster.Pause();
        }
    }
}
