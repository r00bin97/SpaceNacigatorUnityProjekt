using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackToMainMenu : MonoBehaviour {

    private GameObject player;
    public static bool restarted = false;

    public Explosion blow;
    //private GameObject player;

    private void Start()
    {
        blow = GameObject.FindObjectOfType(typeof(Explosion)) as Explosion;
        restarted = false;
    }

    public void Click()
    {
        //GameUI.inSpiel = false;
        //EventManager.PlayerDeath();              
        restarted = true;
        Time.timeScale = 1;
        Invoke("SeRestart", Asteroid.destructionDelay * 3);
        //UnityEngine.SceneManagement.SceneManager.LoadScene(1);

        // UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }

    void SeRestart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(1);
    }
}
