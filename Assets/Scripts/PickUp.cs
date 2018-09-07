using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(CapsuleCollider))]

public class PickUp : MonoBehaviour
{
    static int points = 100;
    [SerializeField] float rotationOffset = 100f;
    bool gotHit = false;
    Transform myT;
    Vector3 randomRotation;

    void Awake()
    {
        myT = transform;
    }

    void Start()
    {
        randomRotation.x = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.y = Random.Range(-rotationOffset, rotationOffset);
        randomRotation.z = Random.Range(-rotationOffset, rotationOffset);
    }

    void Update()
    {
        myT.Rotate(randomRotation * Time.deltaTime);
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            if (!gotHit)
            {
                gotHit = true;
                PickupHit();

            }
        }
        else if (col.tag == "RocketPickup")
        {
            if (!gotHit)
            {
                gotHit = true;
                PickupRocket();
            }
        }
    }

    void HitByRay()
    {
        Debug.Log("PickUp hit by Laser Ray");
    }

    public void PickupHit()
    {
        Debug.Log("Player hit PickUp");
        EventManager.ScorePoints(points);
      //  EventManager.ReSpawnPickup();
        Destroy(gameObject);
    }

    public void PickupRocket()
    {
        Debug.Log("Player hit Rocket PickUp");
        Destroy(gameObject);
    }
}
