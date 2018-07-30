using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour {

	[SerializeField]Transform target;
	[SerializeField]Vector3 defaultDistance = new Vector3 (0f,2f,-10f);
	[SerializeField]float distanceDamp = 10f;
	[SerializeField]float rotationlDamp = 10f;

	public Vector3 velocity = Vector3.one;

	Transform myT;

	void Awake(){
		myT = transform;
	}

	void LateUpdate(){

		//Kamera Variante 1
		SmoothFollow ();


		//Kamera Variante 2
		//Kamera flogt das Target
		//Vector3 toPos = target.position + (target.rotation * defaultDistance);
		//Vector3 curPos = Vector3.Lerp (myT.position, toPos, distanceDamp * Time.deltaTime);
		//myT.position = curPos;

		//Kamera flogt auch bei Rotationen
		//Quaternion toRot = Quaternion.LookRotation(target.position - myT.position, target.up); 
		//Quaternion curRot = Quaternion.Slerp (myT.rotation, toRot, rotationlDamp * Time.deltaTime);
		//myT.rotation = curRot;

	}

	void SmoothFollow(){
		Vector3 toPos = target.position + (target.rotation * defaultDistance);
		Vector3 curPos = Vector3.SmoothDamp (myT.position, toPos, ref velocity, distanceDamp);
		myT.position = curPos;

		myT.LookAt (target, target.up);
	}
}
