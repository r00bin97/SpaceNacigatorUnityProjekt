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


    void OnEnable(){
		EventManager.onPlayerDeath += FindMainCamera;
		EventManager.onStartGame += SelfDestruct;
	}

	void OnDisable(){
		EventManager.onPlayerDeath -= FindMainCamera;
		EventManager.onStartGame -= SelfDestruct;
	}

	void SelfDestruct(){
		Destroy (gameObject); // Zerstört alle Enemies am Spielende, damit man in der nächsten runde bei 0 Enemy anfängt
	}

    private int life = 600;
    void OnCollisionEnter(Collision health)
    {
        if (health.gameObject.tag == "launcher")
            life -= 600;                  
    }

    void HitByRay()
    {
        life -= 150; // Jeder(!) Hit verursacht *3 Schaden ~ -450 pro Treffer.
        GameObject laserSFX = Instantiate(LaserHitEffect, transform.position, Quaternion.identity) as GameObject;
        Destroy(laserSFX, 1f);
        Debug.Log("Enemy Life = " + life);
    }

    void Kill()
    {
        GameObject explosionSFX = Instantiate(blowUpEffect, transform.position, Quaternion.identity) as GameObject; //Particeleffekt getriggert
        GameObject itemDrop = Instantiate(AmmoDrop, transform.position, Quaternion.identity) as GameObject; //Ammo gedropped
        Destroy(explosionSFX, 2f);
        EventManager.ScorePoints(200);
    }

    void Update(){

        if (life <= 0)
        {
            Destroy(gameObject);
            Kill();
        }

        if (!FindTarget ()) {
			return;
		}
		Pathfinding ();
		Turn ();
		Move ();
	}

	// dreht sich zum Playship(dem Spieler)
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

		//Debuging zeigt die Sensoren / hitbox
		Debug.DrawRay (left, transform.forward * detectionDistance, Color.cyan);
		Debug.DrawRay (right, transform.forward * detectionDistance, Color.cyan);
		Debug.DrawRay (up, transform.forward * detectionDistance, Color.cyan);
		Debug.DrawRay (down, transform.forward * detectionDistance, Color.cyan);

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

	bool FindTarget(){
		if (target == null) {
			GameObject temp = GameObject.FindGameObjectWithTag("Player");

			if (temp != null) {
				target = temp.transform;
			}
		}
		if (target == null) {
			return false;
		}
		return true;
	}

	//Wenn der Spieler tot ist bleibt die Kamera stehen
	void FindMainCamera(){
		target = GameObject.FindGameObjectWithTag("MainCamera").transform;
	}
}
