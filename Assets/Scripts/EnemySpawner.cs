using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] float spawnTimer = 5f;


	// Enemy Spawnt hat aber kein Target -> Target finden in EnemyMovement
	void Start(){
		//StartSpawning ();
	}

	void OnEnable(){
		EventManager.onStartGame += StartSpawning; //Spawnen begiinnt erst wenn speielr auf Play gedrückt hat
		EventManager.onPlayerDeath += StopSpawning; // Wenn spieler tot ist sollen keine Weiteren Enemy Spawnen
	}

	void OnDisable(){
		StopSpawning ();
		EventManager.onStartGame -= StartSpawning; 
		//EventManager.onPlayerDeath -= StopSpawning;
	}

	void SpawnEnemy(){
		Instantiate (enemyPrefab, transform.position, Quaternion.identity);
	}

	void StartSpawning(){
		InvokeRepeating ("SpawnEnemy", spawnTimer, spawnTimer);
	}

	void StopSpawning(){
		CancelInvoke ();
	}
}
