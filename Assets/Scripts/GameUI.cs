using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField] GameObject mainMenu;
	[SerializeField] GameObject gameUI;
	[SerializeField]  GameObject playerPrefab;
	[SerializeField] GameObject playerStartPosition;

	void Start(){
		DelayMainMenuDisplay ();
	}

	void OnEnable(){
		EventManager.onStartGame += ShowGameUI; //nach einmal Drücken verschwindet der Playbutton
		EventManager.onPlayerDeath += ShowMainMenu;
	}

	void OnDisable(){
		EventManager.onStartGame -= ShowGameUI; 
		EventManager.onPlayerDeath -= ShowMainMenu;
	}

	void ShowMainMenu(){
		Invoke ("DelayMainMenuDisplay", Asteroid.destructionDelay * 3); // Nach dem Tod dauert er zeit bis das Menu wieder kommt
        //   UnityEngine.SceneManagement.SceneManager.LoadScene(0);  // <-- Add IEnumerator here
    }

    void DelayMainMenuDisplay(){
		mainMenu.SetActive (true);
		gameUI.SetActive (false);
	}

	void ShowGameUI(){
		mainMenu.SetActive (false);
		gameUI.SetActive (true);

		// nach dem klicken auf dem Play Button wird das Raumschiff erzeugt
		Instantiate (playerPrefab, playerStartPosition.transform.position, playerStartPosition.transform.rotation);
    }

/*
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
            GameObject.playerPrefab = false;
            DelayMainMenuDisplay();
    }   
   
*/

}
