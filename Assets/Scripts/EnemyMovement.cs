// Hauptklasse der Gegner

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(EnemyAtack))]
[RequireComponent(typeof(TrailRenderer))]

public class EnemyMovement : MonoBehaviour {

	[SerializeField] Transform target;
	[SerializeField] float rotationlDamp = 0.5f;
	[SerializeField] float movementSpeed = 10f;
	[SerializeField] float rayCastOffset = 2.5f;
	[SerializeField] float detectionDistance = 20f;
    [SerializeField] GameObject LaserHitEffect;
    [SerializeField] GameObject blowUpEffect;
    [SerializeField] GameObject AmmoDrop;

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    public AudioClip hitByLaserSound;
    public AudioClip explosionSound;
   
    bool addOnlyOnce; // Damit Gegner nur einmal in die Liste kommt
    bool inScene; // Um sicher zu sein, dass Gegner auch in Scene ist

    void OnEnable(){
		EventManager.onPlayerDeath += FindMainCamera;
		EventManager.onPlayerDeath += SelfDestruct;
	}

	void OnDisable(){
		EventManager.onPlayerDeath -= FindMainCamera;
		EventManager.onPlayerDeath -= SelfDestruct;
	}

    void Start()
    {
        inScene = true;
        addOnlyOnce = true;
    }

    // Zerstört alle Enemies bei Spielertot, damit es in der nächsten runde bei 0 Enemy anfängt --> Listeneintrag wird 
    void SelfDestruct()
    {
        Destroy (gameObject); 
	}

    private int life = 600;
    void OnCollisionEnter(Collision health)
    {
        if (health.gameObject.tag == "launcher")
            life -= 600;                  
    }

    void HitByRay()
    {
        life -= 150;
        GameObject laserSFX = Instantiate(LaserHitEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(laserSFX, 1f);
        Debug.Log("Enemy Life = " + life);
        AudioSource.PlayClipAtPoint(hitByLaserSound, transform.position);
    }

    //Gegner töten
    void Kill()
    {
        PlayerInput.nearByEnemies.Remove(this); // Entferne Gegner von Liste
        GameObject explosionSFX = Instantiate(blowUpEffect, transform.position, Quaternion.identity) as GameObject; // Particeleffekt getriggert
        randomDrop(); // Randomizer aufrufen
        AudioSource.PlayClipAtPoint(explosionSound, transform.position); // Spiele Audio ab
        Destroy(explosionSFX, 2f); // Entferne Explosion, nach 2 Sekunden
        EventManager.ScorePoints(200); // Spieler erhält 200 Punkte
        Destroy(gameObject);
    }

    // Randomizer, denn nicht jeder Gegner soll Ammo fallen lassen
    void randomDrop()
    {
        int num = Random.Range(0, 60); 
        if (num > 35)
            dropAmmo();
    }

    // Ammo drop
    void dropAmmo()
    {
        GameObject itemDrop = Instantiate(AmmoDrop, transform.position, Quaternion.identity) as GameObject; 
    }

    void Update()
    {
        //Wenn ein Gegner in die Scene kommt, füge diesen einmalig zur Liste hinzu. 
        if (inScene && addOnlyOnce)
        {
            addOnlyOnce = false;
            PlayerInput.nearByEnemies.Add(this);
        }

        if (life <= 0){
            Kill();
        }

        if (!FindTarget ()) {
			return;
		}

		Pathfinding ();
		Turn ();
		Move ();
	}

	// dreht sich zum Spieler
	void Turn(){
		Vector3 pos = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation (pos);
		transform.rotation = Quaternion.Slerp (transform.rotation, rotation, rotationlDamp * Time.deltaTime);
	}

	void Move(){
		transform.position += transform.forward * movementSpeed * Time.deltaTime;
	}

	// Asteroiden suchen und erkennen ggf ausweichen
	void Pathfinding(){
		RaycastHit hit;
		Vector3 raycastOffset = Vector3.zero;

		Vector3 left = transform.position - transform.right * rayCastOffset;
		Vector3 right = transform.position + transform.right * rayCastOffset;
		Vector3 up = transform.position + transform.up * rayCastOffset;
		Vector3 down = transform.position - transform.up * rayCastOffset;

		if (Physics.Raycast (left, transform.forward, out hit, detectionDistance)) {
			raycastOffset += Vector3.right;
		}
		else if (Physics.Raycast (right, transform.forward, out hit, detectionDistance)) {
			raycastOffset -= Vector3.right;
		}
		if (Physics.Raycast (up, transform.forward, out hit, detectionDistance)) {
			raycastOffset -= Vector3.up;
		}
		else if (Physics.Raycast (down, transform.forward, out hit, detectionDistance)) {
			raycastOffset += Vector3.up;
		}

		if (raycastOffset != Vector3.zero) {
			transform.Rotate (raycastOffset * 5f * Time.deltaTime);
		} else {
			Turn ();
		}
	}

    // Findet den Spieler.
    bool FindTarget()
    {
		if (target == null)
        {
			GameObject temp = GameObject.FindGameObjectWithTag("Player");
			if (temp != null) {
				target = temp.transform;
			}
		}
		if (target == null)
        {
			return false;
		}
		return true;
	}

	//Finde die aktive Kamera nach Spielertot.
	void FindMainCamera(){
		target = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
}
