﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {
	[SerializeField] GameObject enemyPrefab;
	[SerializeField] float spawnTimer = 5f;


	void Start(){
		//StartSpawning ();
	}

	void OnEnable(){
		EventManager.onStartGame += StartSpawning; //Spawnen beginnt erst wenn speielr auf Play gedrückt hat
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
