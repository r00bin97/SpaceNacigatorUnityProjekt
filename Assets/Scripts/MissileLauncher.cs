using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;


 
public class MissileLauncher : MonoBehaviour {
    [SerializeField] GameObject missile;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Instantiate(missile, transform.position, transform.rotation);
        }

    }
		
	
}


