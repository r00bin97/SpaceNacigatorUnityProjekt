using UnityEngine;
using System.Collections.Generic;

public class LauncherRocket : LauncherManager
{
    Queue<RaketenSystem> stations;
    int initialMisCount = 1;
    int spawnedMissiles = 0;

    public override int MagazinAnzahl
    {
        get
        {
            return 1;       // 'Get' Anzahl der Magazine. Kein Magazin, muss immer 1 zurückgeben. 
        }
    }     

    protected override void Awake()
    {
        base.Awake();
        stations = new Queue<RaketenSystem>(launchPoints.Count);
    }

    private void Start()
    {
        initialMisCount = raketenAnzahl;
        InitializeStations();
    }

    private void Update()
    {     
        if (spawnedMissiles < initialMisCount)
        {
            foreach (RaketenSystem sta in stations)
            {       
                if (sta.Update(Time.deltaTime))    // sta.update gibt true zurück, wenn eine neue Rakete erzeugt wurde.
                    spawnedMissiles++;
            }
        }
    }

    // Rakete abfeuern.
    public override void Abfeuern(Transform zielPunkt)
    {
        Abfeuern(zielPunkt, Vector3.zero);
    }

    // Rakete abfeuern.
    public override void Abfeuern(Transform zielPunkt, Vector3 velocity)
    {
        RaketenSystem launchingStation = stations.Peek();
        if (launchingStation != null)
        {
            bool launchSuccessful = launchingStation.Launch(zielPunkt, velocity);
            if (launchSuccessful)
            {
                if (fireSource != null)
                { 
                    fireSource.Play();
                }
                // Stellt sich wieder ans Ende der Schlange
                stations.Dequeue();
                stations.Enqueue(launchingStation);
                raketenAnzahl--;
            }
        }
    }

    public override void LauncherZurucksetzen()
    {
        raketenAnzahl = initialMisCount;
        foreach (RaketenSystem station in stations)
            station.ClearHardpoint();

        InitializeStations();
    }

    //Warteschlange für Stationen.
    private void InitializeStations()
    {
        stations.Clear();
        spawnedMissiles = 0;

        // Raketenanzahl darf nicht geringer eingestellt sein als launchPoints vorhanden sind.
        foreach (Transform point in launchPoints)
        {
            RaketenSystem newStation = new RaketenSystem(verzögerung, rocketPrefab, point, spielerSchiff);
            stations.Enqueue(newStation);
            spawnedMissiles++;
        }
    }

    internal class RaketenSystem
    {
        public LauncherMissile loadedMissile;
        LauncherMissile prefab;
        Transform launchPoint;
        Transform spielerSchiff;
        float reloadTime = 1.0f;
        float cooldown = 0.0f;

        // Zeitinterval bis nachgeladen wurde. -> launchPoint = Punkt, an welchem erzeugte Raketen angehängt werden 
        public RaketenSystem(float reloadTime, LauncherMissile missilePrefab, Transform launchPoint, Transform spielerSchiff)
        {
            prefab = missilePrefab;
            this.reloadTime = reloadTime;
            this.launchPoint = launchPoint;
            this.spielerSchiff = spielerSchiff;
            cooldown = 0.0f;
            loadedMissile = ErzeugeRackete(launchPoint);
        }

        // Cooldown automatisches Nachladen. Frameweise mit deltaTime. Gibt true zurück, wenn eine Rakete erzeugt wurde.  
        public bool Update(float deltaTime)
        {
            bool spawnedNewMissle = false;
            if (loadedMissile == null)
            {
                cooldown -= deltaTime;
                if (cooldown <= 0.0f)    // Wenn Nachladen fertig, neue Rakete erzeugen.
                {
                    loadedMissile = ErzeugeRackete(launchPoint);
                    spawnedNewMissle = true;
                }
            }
            return spawnedNewMissle;
        }

        // Feuert immer gerade ohne Zielpunkt.
        public bool Launch(Transform zielPunkt, Vector3 inheritedVelocity)
        {
            bool successfulLaunch = false;

            if (loadedMissile != null)
            {
                loadedMissile.Abfeuern(zielPunkt, inheritedVelocity);
                loadedMissile = null;
                cooldown = reloadTime;
                successfulLaunch = true;
            }
            return successfulLaunch;
        }

        public void ClearHardpoint()
        {
            if (loadedMissile != null)
            {
                Destroy(loadedMissile.gameObject);
                loadedMissile = null;
            }
            cooldown = 0.0f;
        }

        // Erzeugt eine neue Rakete.
        private LauncherMissile ErzeugeRackete(Transform newParent)
        {
            LauncherMissile rakete = Instantiate(prefab, newParent);
            rakete.spielerSchiff = spielerSchiff;

            if (rakete.attachPoint != null)
            {
                rakete.transform.localPosition = -rakete.attachPoint.localPosition;
                rakete.transform.localEulerAngles = -rakete.attachPoint.localEulerAngles;
            }
            else
            {
                rakete.transform.localPosition = Vector3.zero;
                rakete.transform.localEulerAngles = Vector3.zero;
            }
            return rakete;
        }
    }
}