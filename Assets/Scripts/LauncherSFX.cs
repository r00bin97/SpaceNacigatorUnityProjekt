using UnityEngine;

[RequireComponent(typeof(LauncherMissile))]
public class LauncherSFX : MonoBehaviour
{
    new Transform transform;
    public Transform trailPos;  
    public TrailRenderer trailPrefab;
    public ParticleSystem particleTrailPrefab;
    public ParticleSystem explosionFXPrefab;   

    public UnityEngine.Audio.AudioMixerGroup mixerGroup;
    public AudioClip fireClip;
    public float fireVolume = 1.0f;
    public float fireMinDistance = 25.0f;
    public float fireMaxDistance = 500.0f;
    protected AudioSource fireSource;
    public AudioClip loopClip;
    public float loopVolume = 1.0f;
    public float loopMinDistance = 30.0f;
    public float loopMaxDistance = 500.0f;
    protected AudioSource loopSource;

    TrailRenderer trail;
    ParticleSystem particleTrail;
    LauncherMissile missile;
    LauncherRemoveFX effectCleaner;

    public bool trailAlwaysActive = true;
    public bool SelfDestruct = false;
    bool antriebAktiviert = false;

    private void Awake()
    {
        transform = GetComponent<Transform>();
        missile = GetComponent<LauncherMissile>();

        if (fireClip != null)
        {
            fireSource = gameObject.AddComponent<AudioSource>();
            fireSource.clip = fireClip;
            fireSource.minDistance = fireMinDistance;
            fireSource.maxDistance = fireMaxDistance;
            fireSource.loop = false;
            fireSource.dopplerLevel = 0.0f;
            fireSource.spatialBlend = 1.0f;
            fireSource.volume = fireVolume;
            fireSource.pitch = Random.Range(0.9f, 1.3f);
            fireSource.outputAudioMixerGroup = mixerGroup ?? null;
            fireSource.Stop();
        }

        if (loopClip != null)
        {
            loopSource = gameObject.AddComponent<AudioSource>();
            loopSource.clip = loopClip;
            loopSource.minDistance = loopMinDistance;
            loopSource.maxDistance = loopMaxDistance;
            loopSource.loop = true;
            loopSource.dopplerLevel = 1.0f;
            loopSource.volume = loopVolume;
            loopSource.spatialBlend = 1.0f;
            loopSource.outputAudioMixerGroup = mixerGroup ?? null;
            loopSource.Stop();
        }
    }

    private void Start()
    {
        // Tue nichts, wenn keine Trail angegeben ist.
        if (trailPrefab != null || particleTrailPrefab != null)
        {
            // Suche nach Referenzpunkten, um Trail zu spawnen. Nutze Objekt-Transformation, wenn keine Referenz vorhanden.
            if (trailPos == null)
            {
                trailPos = transform.Find("TrailFx");

                if (trailPos == null)
                    trailPos = transform;
            }

            // Initialisiere Trail
            if (trailPrefab != null)
            {
                trail = Instantiate(trailPrefab, trailPos);
                trail.transform.localPosition = Vector3.zero;
                trail.transform.localEulerAngles = Vector3.zero;
                trail.enabled = false;
            }

            // Initialisiere ParticleSystem
            if (particleTrailPrefab != null)
            {
                particleTrail = Instantiate(particleTrailPrefab, trailPos);
                particleTrail.transform.localPosition = Vector3.zero;
                particleTrail.transform.localEulerAngles = Vector3.zero;
                particleTrail.Stop();

                // Hänge effectCleaner an das gameObjekt, um Effekte auch wieder abstellen zu können.
                if (particleTrail.GetComponent<LauncherRemoveFX>() == null)
                    effectCleaner = particleTrail.gameObject.AddComponent<LauncherRemoveFX>();
            }
        }
    }

    private void Update()
    {
        // starte Effekte, wenn Antrieb aktiviert wurde.
        if (!antriebAktiviert && missile.AntriebAktiv)
        {
            antriebAktiviert = true;

            // Spiele Audio ab.
            if (fireSource != null)
                fireSource.Play();
            if (loopSource != null)
                loopSource.Play();

            // Trail und Partikel setzen.
            if (trail != null)
                trail.enabled = true;
            else if (particleTrail != null)
                particleTrail.Play();                  
        }

        // Trail "abschalten", wenn Antrieb deaktiviert wurde.
        if (!trailAlwaysActive && antriebAktiviert && !missile.AntriebAktiv)
            StopTrail();
    }
    
    public void Explode()
    {
        StopTrail();

        if (explosionFXPrefab != null)
        {
            ParticleSystem explode = GameObject.Instantiate(explosionFXPrefab);
            explode.transform.position = transform.position;
            explode.transform.rotation = transform.rotation;

            // Gibt der Explosion die nötige Componente um sich selbst zu zerstören.
            LauncherRemoveFX remove = explode.GetComponent<LauncherRemoveFX>();
            if (remove == null)    
                remove = explode.gameObject.AddComponent<LauncherRemoveFX>();

            remove.readyToDestroy = true;
        }
    }

    // Trail & Partikel abschalten.
    private void StopTrail()
    {
        if (fireSource != null)
            fireSource.Stop();
        if (loopSource != null)
            loopSource.Stop();

        if (trail != null)
        {         
            if (trail.gameObject.activeSelf)
            {
                trail.transform.parent = null;
                trail.autodestruct = true;
            }
            else
                GameObject.Destroy(trail);
        }

        if (particleTrail != null)
        {
            if (particleTrail.gameObject.activeSelf)
            {
                particleTrail.transform.parent = null;
                particleTrail.Stop();
                effectCleaner.readyToDestroy = true;
            }
            else
                GameObject.Destroy(particleTrail);
        }
    }
}