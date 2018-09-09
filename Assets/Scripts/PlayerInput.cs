using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] Laser[] laser;
    public Transform zielPunkt;
    private bool EnteredTrigger;
    private GameObject rocketPickup;
    new Rigidbody rigidbody;
    int selectedLauncherGroup = 0;
    private bool inPause = false;
    public AudioClip laserSound;
    public AudioClip switchWeapon;

    Queue<LauncherManager>[] launchers;
    LauncherManager[] allLaunchers;

    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();

        launchers = new Queue<LauncherManager>[2];
        for (int i = 0; i < 2; i++)
            launchers[i] = new Queue<LauncherManager>();
    }

    void Start()
    {

        allLaunchers = GetComponentsInChildren<LauncherManager>();
        rocketPickup = GameObject.FindWithTag("RocketPickup");

        // Register launchers
        foreach (LauncherManager launcher in allLaunchers)
        {
            if (launcher.name.StartsWith("Right"))
            {
                launchers[0].Enqueue(launcher);
            }

            else if (launcher.name.StartsWith("Left"))
            {
                launchers[1].Enqueue(launcher);
            }
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.CompareTag("Player"))
            EnteredTrigger = true;
        foreach (LauncherManager launcher in allLaunchers)
            launcher.LauncherZurucksetzen();
        UpdateAmmoCounters();

    }

    void Update()
    {
        //Laser
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AudioSource.PlayClipAtPoint(laserSound, transform.position);
            foreach (Laser l in laser)
            {
                Vector3 pos = transform.position + (transform.forward * l.Distance);
                l.FireLaser();
            }
        }

        // Launcher wechseln.
        if (Input.GetButtonDown("Fire2") && inPause == false)
        {
            AudioSource.PlayClipAtPoint(switchWeapon, transform.position);
            selectedLauncherGroup++;
            if (selectedLauncherGroup >= 2)
                selectedLauncherGroup = 0;
        }

        // Ausgewählten Launcher abfeuern
        if (selectedLauncherGroup == 1)
        {
            if (Input.GetButton("Fire1") && inPause == false)
                FireWeapon();
        }
        else
        {
            if (Input.GetButtonDown("Fire1") && inPause == false)
                FireWeapon();
        }
        UpdateAmmoCounters();
    }

    private void FireWeapon()
    {
        // Nechsten Launcher abfeuern und an Warteschlange anstellen
        if (launchers[selectedLauncherGroup].Count > 0)
        {
            LauncherManager temp = launchers[selectedLauncherGroup].Dequeue();
            temp.Abfeuern(zielPunkt, rigidbody.velocity);
            launchers[selectedLauncherGroup].Enqueue(temp);
        }
    }

      private void UpdateAmmoCounters() // Set back to private
      {
        // Ammo counter updaten 
        int raketenAnzahl = 0;
          int rocketCount = 0;
          int rocketMagazine = 0;

          foreach (LauncherManager launcher in allLaunchers)
          {
              if (launcher.name.StartsWith("Right"))
                    raketenAnzahl += launcher.raketenAnzahl;

              else if (launcher.name.StartsWith("Left"))
              {
                  rocketCount += launcher.raketenAnzahl;
                  rocketMagazine += launcher.MagazinAnzahl;
              }
          }
      }

}





