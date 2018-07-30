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


	void Awake(){
		lr = GetComponent<LineRenderer> ();
		laserLight = GetComponent<Light> ();
	}

	void Start(){
		lr.enabled = false;
		laserLight.enabled = false;
	}

	//void Update(){
	//	FireLaser (transform.forward * maxDistance);
	//}


	public void FireLaser(Vector3 targetPosition){

		if (canFire) {
			lr.SetPosition (0, transform.position);
			lr.SetPosition (1, targetPosition);
			lr.enabled = true;
			laserLight.enabled = true;
			canFire = false;
		}

		Invoke ("TurnOffLaser", laserOffTime);
	}


	void TurnOffLaser(){
		lr.enabled = false;
		laserLight.enabled = false;
		canFire = true;
	}

	public float Distance
	{
		get { return maxDistance; }
	}

}
