using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Referenz File
// Obsolete --- > Delete this file 

public class MissileHoming : MonoBehaviour
{
    [SerializeField] float missileVelocity = 300f;
    [SerializeField] float turn = 20f;
    [SerializeField] Rigidbody homingMissile;
   // [SerializeField] float fuseDelay;
    [SerializeField] GameObject missileMod;
    [SerializeField] ParticleSystem SmokePrefab;
    //   [SerializeField] AudioClip missileClip;

    private Transform target;

 
    void Start()
    {
        homingMissile = transform.GetComponent<Rigidbody>(); 
        Fire();

    }

    void FixedUpdate()

    {
        if (target == null || homingMissile == null)
            return;

        homingMissile.velocity = transform.forward * missileVelocity;

        Quaternion targetRotation = Quaternion.LookRotation(target.position - transform.position);
        homingMissile.MoveRotation(Quaternion.RotateTowards(transform.rotation, targetRotation, turn));

    }

    /*   IEnumerator Fire()

       {
           yield return new WaitForSeconds(fuseDelay);
           //AudioSource.PlayClipAtPoint(missileClip, transform.position);

           var distance = Mathf.Infinity;

           foreach (var go in GameObject.FindGameObjectsWithTag("target"))
           {
               var diff = (go.transform.position - transform.position).sqrMagnitude;

               if (diff < distance)
               {
                   distance = diff;
                   target = go.transform;
               }

           }

       }
   */

    void Fire()

    {
        //yield return new WaitForSeconds(fuseDelay);
        //AudioSource.PlayClipAtPoint(missileClip, transform.position);

        float distance = Mathf.Infinity;

        foreach (var go in GameObject.FindGameObjectsWithTag("Enemy"))
        {
            float diff = (go.transform.position - transform.position).sqrMagnitude;

            if (diff < distance)
            {
                distance = diff;
                target = go.transform;
            }

        }

    }

    IEnumerator WaitTime() {
        yield return new WaitForSeconds(5);
    }


    void OnCollisionEnter(Collision Collision)
    {

        if (Collision.gameObject.name == "Cube")
        {
            SmokePrefab.emissionRate = 0.0f;
            Destroy(missileMod.gameObject);
            WaitTime();
            Destroy(gameObject);
        }

    }

}