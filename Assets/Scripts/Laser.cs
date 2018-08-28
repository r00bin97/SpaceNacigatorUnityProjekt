using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Light))]
public class Laser : MonoBehaviour {

	Light laserLight;
	LineRenderer lr;
	bool canFire;

	[SerializeField]float laserOffTime = .5f;
	[SerializeField]float maxDistance = 300f;
	[SerializeField] float fireDelay = 10f; //cooldown zwischen den schüssen
    public GameObject destroyedVersion;
    GameObject asteriodHit;


    void Awake(){
		lr = GetComponent<LineRenderer> ();
		laserLight = GetComponent<Light> ();
	}

	void Start(){
		lr.enabled = false;
		laserLight.enabled = false;
		canFire = true;
	}

	void Update(){
		Debug.DrawRay (transform.position, transform.TransformDirection (Vector3.forward) * maxDistance, Color.yellow);
	}

	//Shooting 
	Vector3 CastRay(){
		RaycastHit hit;

		Vector3 fwd = transform.TransformDirection (Vector3.forward) * maxDistance;

		// Objekt getroffen
		if (Physics.Raycast (transform.position, fwd, out hit)) {
			Debug.Log ("hit:" + hit.transform.name);
     
			SpawnExplosion (hit.point, hit.transform);

            // Destruction of Asteriods // ToDo Fix Error
            asteriodHit = GameObject.FindWithTag("Asteroid");
            Instantiate(destroyedVersion, hit.transform.position, transform.rotation);
            //Destroy(asteriodHit);

            return hit.point;

		} 

		Debug.Log ("Miss");
		return transform.position + (transform.forward * maxDistance);
			
	}

	void SpawnExplosion(Vector3 hitPosition, Transform target){

		Explosion temp = target.GetComponent<Explosion> ();

		if (temp != null) {
			temp.AddForce (hitPosition, transform);
		}
			
	}

	//Shooting vom Player
	public void FireLaser(){
		Vector3 pos = CastRay ();
		FireLaser (pos);
	}

	//Shooting von Enemys
	public void FireLaser(Vector3 targetPosition, Transform target = null){
		
		if (canFire) {

			if (target != null) {
				SpawnExplosion (targetPosition, target);
			}

			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, targetPosition);
			lr.enabled = true;
			laserLight.enabled = true;
			canFire = false;
			Invoke ("TurnOffLaser", laserOffTime);
			Invoke ("CanFire", fireDelay);
		}
	}


	void TurnOffLaser(){
		lr.enabled = false;
		laserLight.enabled = false;

	}

	public float Distance
	{
		get { return maxDistance; }
	}

	void CanFire(){
		canFire = true;
	}

}
