using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield : MonoBehaviour {

	[SerializeField] int maxHealth = 10;
	[SerializeField] int curHealth;
	[SerializeField] float regenerationRate = 2f;
	[SerializeField] int regenerationAmount = 1;
    private AudioSource Source;
    public AudioClip clip;
    public AudioClip alarmDamage;
    public AudioClip ammoFull;
    private bool alarmPlay = false;

    public int pickUpCount = 0;

    void Awake()
    {
        Source = GetComponent<AudioSource>();
    }

    void Start(){
		curHealth = maxHealth;
        InvokeRepeating ("Regenerate", regenerationRate, regenerationRate);
	}

    void Regenerate(){
		if (curHealth < maxHealth) {
			curHealth += regenerationAmount;
		}
		if (curHealth > maxHealth) {
			curHealth = maxHealth;
			CancelInvoke ();
		}
		EventManager.TakeDamage (curHealth/(float)maxHealth);
       // Source.PlayOneShot(alarmDamage, 5f);
    }

	public void TakeDamage(int dmg = 1){
		curHealth -= dmg;

		if (curHealth < 0) {
			curHealth = 0;
		}

		EventManager.TakeDamage (curHealth/(float)maxHealth);

        // Play Alarm
        if (curHealth < maxHealth-2 && alarmPlay == false)
        {
            Source.PlayOneShot(alarmDamage, 0.7f);
            alarmPlay = true;
        }
        if (curHealth >= maxHealth-1)
        {
            alarmPlay = false;
        }

        // Destroy Player Ship
        if (curHealth < 1) {
			EventManager.PlayerDeath ();
			GetComponent<Explosion> ().BlowUp ();
		}
			
	}

    void OnTriggerEnter(Collider colide)
    {
        if (colide.tag == "Pickup")
        {
            AudioSource.PlayClipAtPoint(clip, transform.position);
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
    }  

}
