using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleLights : MonoBehaviour {

    public AudioClip lightOn;
    public AudioClip lightOff;
    private bool lightON = false;

    // Hohle Script als GameObject, Parent Objekt muss ausgewählt werden (in Editor).
    public GameObject lightShaftScript; 


    void Start ()
    {
        this.GetComponent<Light>().enabled = false;
        lightShaftScript.GetComponent<LightShafts>().enabled = false;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.B) && lightON == false && GameUI.inSpiel == false)
        {           
            this.GetComponent<Light>().enabled = true;
            lightShaftScript.GetComponent<LightShafts>().enabled = true;
            AudioSource.PlayClipAtPoint(lightOn, transform.position);
            lightON = true;
        }
        else if (Input.GetKeyDown(KeyCode.B) && GameUI.inSpiel == false)
        {           
            this.GetComponent<Light>().enabled = false;
            lightShaftScript.GetComponent<LightShafts>().enabled = false;
            AudioSource.PlayClipAtPoint(lightOff, transform.position);
            lightON = false;
        }
    }   
}
