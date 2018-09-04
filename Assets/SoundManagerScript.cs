using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour {

    public static AudioClip lasersound, shootandExplosion, galaxyhintergrund, raketa;
    static AudioSource audioSrc;

	void Start () {

        lasersound = Resources.Load < AudioClip > ("laser");
        shootandExplosion = Resources.Load<AudioClip>("shootandexplosion");
        galaxyhintergrund = Resources.Load<AudioClip>("galaxyhintergrund3spiel");
        raketa = Resources.Load<AudioClip>("raketa");

        audioSrc = GetComponent<AudioSource>();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound (string clip)
    {
       switch (clip)
        {
            case "laser":
                audioSrc.PlayOneShot(lasersound);
                break;
            case "shootandexplosion":
                audioSrc.PlayOneShot(shootandExplosion);
                break;
            case "galaxyhintergrund3spiel":
                audioSrc.PlayOneShot(galaxyhintergrund);
                break;
            case "raketa":
                audioSrc.PlayOneShot(raketa);
                break;
        }
    }

}
