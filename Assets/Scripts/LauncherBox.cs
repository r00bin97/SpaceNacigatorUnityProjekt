using UnityEngine;

public class LauncherBox : LauncherManager
{
    public float streuung = 0.0f;        // Höhere Nummer = Mehr Streuung
    public int magazinAnzahl = 1;               // Anzahl zusätzlicher Magazine - optional
    public float magNachladeZeit = 6.0f;     // Zeit bis Magazin nachgeladen wurde
    int boxAnzahl = 0;
    int startRaketen = 1;
    int startMagazine = 1;

    private void Start()
    {
        startRaketen = raketenAnzahl;
        startMagazine = magazinAnzahl;
    }

    private void Update()
    {
        if (nachladenCooldown > 0.0f)                               // Während Cooldown
        {
            nachladenCooldown -= Time.deltaTime;
            if (nachladenCooldown <= 0.0f)
                nachladenCooldown = 0.0f;
        }

        if (magazinAnzahl > 0 && magazinNachladenCooldown > 0.0f)  // Während Nachladen
        {
            magazinNachladenCooldown -= Time.deltaTime;
            if (magazinNachladenCooldown <= 0.0f)                  // Wenn Nachladen fertig ist.
            {
                raketenAnzahl = startRaketen;
                nachladenCooldown = 0.0f;
                magazinAnzahl--;
            }
        }
    }

    public override void Abfeuern(Transform zielPunkt)                   // Rakete abfeuern. Sofern kein Target übergeben, schieße gerade.
    {
        Abfeuern(zielPunkt, Vector3.zero);
    }

    public override void Abfeuern(Transform zielPunkt, Vector3 velocity) // Rakete abfeuern, schieße gerade ohne Zielpunkt.
    {
        if (raketenAnzahl > 0 && nachladenCooldown <= 0.0f && magazinNachladenCooldown <= 0.0f)
        {
            if (fireSource != null)
                fireSource.Play();

            Vector3 deviation = UnityEngine.Random.insideUnitCircle * (streuung * Mathf.Deg2Rad);
            deviation = launchPoints[boxAnzahl].TransformDirection(deviation);
            Vector3 randomizedForward = launchPoints[boxAnzahl].forward + deviation;
            Quaternion randomizedRotation = Quaternion.LookRotation(randomizedForward);

            LauncherMissile rakete = ErzeugeRackete(launchPoints[boxAnzahl].position, randomizedRotation);
            rakete.zielPunkt = zielPunkt;
            rakete.Abfeuern(zielPunkt, velocity);
            nachladenCooldown = verzögerung;
            raketenAnzahl--;

            boxAnzahl++;
            if (boxAnzahl >= launchPoints.Count)
                boxAnzahl = 0;
            // Reload
            if (raketenAnzahl <= 0)
                Nachladen();
        }
    }

    public override void LauncherZurucksetzen()     // Setzt Launcher zurück.
    {
        nachladenCooldown = 0.0f;
        magazinNachladenCooldown = 0.0f;
        raketenAnzahl = startRaketen;
        magazinAnzahl = startMagazine;
    }

    public void Nachladen()            // Funktion für manuelles Nachladen
    {
        magazinNachladenCooldown = magNachladeZeit;
    }

    public override int MagazinAnzahl       // 'Get' Anzahle der Magazine des Launchers. 
    {
        get{return magazinAnzahl; }
    }

    private LauncherMissile ErzeugeRackete(Vector3 position, Quaternion rotation)      // Erzeugt Missile und feuert diese ab. 
    {
        LauncherMissile rakete = Instantiate(rocketPrefab) as LauncherMissile;
        rakete.spielerSchiff = spielerSchiff;
        rakete.transform.position = position;
        rakete.transform.rotation = rotation;
        return rakete;
    }
}