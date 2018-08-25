using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Obsolete --- > Delete this file

public class MissileHomingP : MonoBehaviour
{

    GameObject[] enemiesInScene;//Array That Holds All Enemies In Scene
    GameObject target;//Selected Target

    bool moveToEnemy; //When True, Object Moves To Enemy

    void Start()
    {
        moveToEnemy = false;//Can't Attack Until Number Of Enemies Is Determined

        enemiesInScene = GameObject.FindGameObjectsWithTag("Enemy"); //Find All Enemies In Scene With This Tag

        //If There Is At Least One Enemy, Select A Target
        if (enemiesInScene.Length > 0)
        {
            SelectTarget();
            StartCoroutine(GoToEnemy());
        }
        else
        {
            //If No Enemies, Self Destruct
            StartCoroutine(SelfDestruct());
        }
    }

    void SelectTarget()
    {
        int rand = Random.Range(0, enemiesInScene.Length); //Create Random Number
        target = enemiesInScene[rand]; //Randomly Select A Target From The List Of Enemies
    }

    // Update is called once per frame
    void Update()
    {
        if (moveToEnemy)
        {
            //Even If The Enemy Moves, The Attack Will Follow It
            Vector3 adjustedTargetPosition = new Vector3(target.transform.position.x, target.transform.position.y + 2, target.transform.position.z);

            transform.position = Vector3.MoveTowards(transform.position, adjustedTargetPosition, 20 * Time.deltaTime);
        }

        if (!moveToEnemy)
        {
            //Go Upward Until Time To Move To Target, If No Target Is Selected, Continue To Rise   
            Vector3 positionToRiseTo = new Vector3(transform.position.x, transform.position.y + 20, transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, positionToRiseTo, Time.deltaTime);
        }
    }

    //Pause Before Moving To Selected Target
    IEnumerator GoToEnemy()
    {
        yield return new WaitForSeconds(1.5f);
        moveToEnemy = true;
    }

    //If No Targets Available, Destory Missle
    IEnumerator SelfDestruct()
    {
        yield return new WaitForSeconds(1);
        GameObject.Destroy(this.gameObject);
    }
}