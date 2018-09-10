using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour {
    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject menuImage;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject gameHud;
    [SerializeField] GameObject gameUI;
	[SerializeField] GameObject playerPrefab;
    [SerializeField] GameObject playerPrefab2;
    [SerializeField] GameObject playerStartPosition;

    public Button shipSelectButton;
    public Button shipSelectButton2;

    private bool newStart = false; 
    private bool inSpiel = false;
    private bool playerDead = false;
    private bool spielerShiff2 = false;


    void Start(){

        Button btn1 = shipSelectButton.GetComponent<Button>();
        btn1.onClick.AddListener(selectShip1);

        Button btn2 = shipSelectButton2.GetComponent<Button>();
        btn2.onClick.AddListener(selectShip2);

        if (newStart == false)
        {
            MainMenu();
            newStart = true;
            DelayMainMenuDisplay();
        }
        else
        {
            DelayMainMenuDisplay();
        }		
	}

    void selectShip1(){
        spielerShiff2 = false;
        Debug.Log("Ship2 selected!= ");
        Debug.Log(spielerShiff2);
        Instantiate(playerPrefab, playerStartPosition.transform.position, playerStartPosition.transform.rotation);
    }

    void selectShip2(){
        spielerShiff2 = true;
        Debug.Log("Ship2 selected!= ");
        Debug.Log(spielerShiff2);
        Instantiate(playerPrefab2, playerStartPosition.transform.position, playerStartPosition.transform.rotation);
    }

    void OnEnable(){
		EventManager.onStartGame += ShowGameUI;
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

    void MainMenu()
    {
        menuImage.SetActive(true);
    }


    void DelayMainMenuDisplay(){
        mainMenu.SetActive (true);
		gameUI.SetActive (false);
        gameHud.SetActive(false);
        Cursor.visible = true;
        playerDead = true;
    }

	void ShowGameUI(){
        menuImage.SetActive(false);
        mainMenu.SetActive (false);
		gameUI.SetActive (true);
        gameHud.SetActive(true);
        Cursor.visible = false;
        playerDead = false;
    }

    void Update()
    {
        {
            if (Input.GetKeyDown(KeyCode.Escape) && inSpiel == false && playerDead == false)
            {
                // Halte Zeit an, zeige Menü, verstecke Fadenkreuz sowie Spielcursor und zeige Mauscursor. 
                Time.timeScale = 0;
                pauseMenu.SetActive(true);
                gameHud.SetActive(false);
                Cursor.visible = true;
                inSpiel = true;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && playerDead == false)
            {
                Time.timeScale = 1;
                pauseMenu.SetActive(false);
                gameHud.SetActive(true);
                Cursor.visible = false;
                inSpiel = false;
            }
            else if (Input.GetKeyDown(KeyCode.Escape) && playerDead == true)
            {
                return;
            }
        }
    }

 }
