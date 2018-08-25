using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpMissile : PlayerInput
{
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

     //   UpdateAmmoCounters();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.transform.CompareTag("Player"))
        {
            if (!gotHit)
            {
                gotHit = true;
                PickupRocket();
            }

        }
    }

    public void PickupRocket()
    {
        Debug.Log("Player hit Rocket PickUp");
        Destroy(gameObject);
     //   UpdateAmmoCounters();
    }


}


// ToDo- Integrate in PicUpSkript