﻿// Shield = Lebensenergie des Spielers. Beinhaltet auch PickUp interaktion.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

    [SerializeField] int maxHealth = 10;               // Maximale Lebensenergie 
	[SerializeField] int curHealth;                    // Aktuelle Lebensenergie
	[SerializeField] float regenerationRate = 2f;      // Regenerations Rate
	[SerializeField] int regenerationAmount = 1;       // Menge der regenerierten Energie pro Sekunde 

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    static int points = 100;
    private AudioSource Source;
    public AudioClip clip;
    public AudioClip alarmDamage;
    public AudioClip takeDamage;
    public AudioClip playerHit;
    public AudioClip ammoFull;
    private bool alarmPlay = false;

    public int pickUpCount = 0;

    void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    void Start(){
		curHealth = maxHealth;  // Setze Lebensenergie auf Maximum bei Spielstart.                                         
        InvokeRepeating ("Regenerate", regenerationRate, regenerationRate);
	}

    // Player Regeneration
    void Regenerate(){
		if (curHealth < maxHealth && curHealth > 0) {
			curHealth += regenerationAmount;
		}
		if (curHealth > maxHealth) {
			curHealth = maxHealth;
			CancelInvoke ();
		}
		EventManager.TakeDamage (curHealth/(float)maxHealth);
    }

    // Schaden abziehen
    public void TakeDamage(int dmg = 1)
    {
		curHealth -= dmg;

		if (curHealth <= 0)
			curHealth = 0;

		EventManager.TakeDamage (curHealth/(float)maxHealth);

        // PlayDamageSound (Fire)
        if (curHealth < maxHealth)
        { 
            Source.PlayOneShot(playerHit, 0.2f);
            Source.PlayOneShot(takeDamage, 0.7f);
        }

        // Spiele Alarm Sound ab
        if (curHealth < maxHealth-2 && alarmPlay == false)
        {
            Source.PlayOneShot(alarmDamage, 0.7f);
            alarmPlay = true;
        }

        // Stope Alarm Sound
        if (curHealth >= maxHealth-1)
            alarmPlay = false;

        // Destroy Player Ship
        if (curHealth < 1)
        {
			EventManager.PlayerDeath ();
			GetComponent<Explosion> ().BlowUp ();
		}
			
	}

    // Einsammlung von Pickups 
    void OnTriggerEnter(Collider colide)
    {
        if (colide.tag == "Pickup")
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
            EventManager.ScorePoints(points);
            pickUpCount++;

            if(pickUpCount >= 3)
            { 
                EventManager.ReSpawnPickup();
                Debug.Log("Respawn Pickup ");
                pickUpCount = 0;
            }

            Debug.Log("pickupCount: " + pickUpCount);
        }

        else if (colide.tag == "RocketPickup")
        {
            AudioSource.PlayClipAtPoint(ammoFull, transform.position);            
        }
        else if (colide.tag == "Handplaced")
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
            EventManager.ScorePoints(points);
        }
    }  

}
