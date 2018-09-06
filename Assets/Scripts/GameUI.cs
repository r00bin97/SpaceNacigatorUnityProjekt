using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameHud;
    [SerializeField] GameObject gameUI;
	[SerializeField] GameObject playerPrefab;
	[SerializeField] GameObject playerStartPosition;

    private bool inSpiel = false;

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
        //   UnityEngine.SceneManagement.SceneManager.LoadScene(1);  // <-- Add IEnumerator here
    }

    void DelayMainMenuDisplay(){
		mainMenu.SetActive (true);
		gameUI.SetActive (false);
        gameHud.SetActive(false);
    }

	void ShowGameUI(){
		mainMenu.SetActive (false);
		gameUI.SetActive (true);
        gameHud.SetActive(true);

        // nach dem klicken auf dem Play Button wird das Raumschiff erzeugt
        Instantiate (playerPrefab, playerStartPosition.transform.position, playerStartPosition.transform.rotation);
    }


    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Escape) && inSpiel == false)
            {
                // Halte Zeit an, zeige Menü, verstecke Fadenkreuz sowie Spielcursor und zeige Mauscursor. 
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                gameHud.SetActive(false);
                Cursor.visible = true;
                inSpiel = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                gameHud.SetActive(true);
                Cursor.visible = false;
                inSpiel = false;
            }
        }
    }

 }
