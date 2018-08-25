using UnityEngine;
using System.Collections.Generic;

public abstract class LauncherManager : MonoBehaviour
{
    new Transform transform;
    public List<Transform> launchPoints;
    protected float nachladenCooldown = 0.0f;
    protected float magazinNachladenCooldown = 0.0f;
    public LauncherMissile rocketPrefab;
    public Transform spielerSchiff;
    public int raketenAnzahl = 1;
    public float verzögerung = 6.0f;

    // Audio: -> ToDo: Erzeugtes Audio an entsprechende Mixer Group hängen.
    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    public AudioClip fireClip;
    public float fireVolume = 1.0f;
    public float minDistance = 10.0f;
    public float maxDistance = 200.0f;
    protected AudioSource fireSource;

    abstract public int MagazinAnzahl
    {
        get;
    }

    protected virtual void Awake()
    {
        transform = GetComponent<Transform>();
        if (launchPoints.Count == 0)
        {
            Transform[] potentialHPs = GetComponentsInChildren<Transform>();
            for (int i = 0; i < potentialHPs.Length; i++)
            {
                if (potentialHPs[i].name.StartsWith("Hp"))
                    launchPoints.Add(potentialHPs[i]);
            }

            // Wenn keine Punkte gefunden wurden, erzeuge direkt an Launcher.
            if (launchPoints.Count == 0)
                launchPoints.Add(transform);
        }

        if (fireClip != null)
        {
            fireSource = gameObject.AddComponent<AudioSource>();
            fireSource.clip = fireClip;
            fireSource.minDistance = minDistance;
            fireSource.maxDistance = maxDistance;
            fireSource.loop = false;
            fireSource.dopplerLevel = 0.0f;
            fireSource.spatialBlend = 1.0f;
            fireSource.volume = fireVolume;
            fireSource.pitch = UnityEngine.Random.Range(0.9f, 1.3f);
            fireSource.outputAudioMixerGroup = mixerGroup ?? null;
            fireSource.Stop();
        }
    }

    // Abstrakte Methode um Launcher zurück zu setzen.
    abstract public void LauncherZurucksetzen();

    // Abstrakte Methode zum Abfeuern.
    abstract public void Abfeuern(Transform zielPunkt);

    // Abstrakte Methode zum Abfeuern - mit velocity.
    abstract public void Abfeuern(Transform zielPunkt, Vector3 velocity);
}