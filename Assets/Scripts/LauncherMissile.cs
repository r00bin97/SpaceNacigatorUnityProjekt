using UnityEngine;
using System.Collections;

public enum UpdateType
{
    FixedUpdate,
    Update
}

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(LauncherSFX))]

public class LauncherMissile : MonoBehaviour
{
    new Rigidbody rigidbody;
    new CapsuleCollider collider;
    new Transform transform;
    
    public Transform zielPunkt;
    public Transform spielerSchiff;
    public Transform attachPoint;
    public UpdateType movementUpdateCycle = UpdateType.Update;
    public UpdateType targetUpdateCycle = UpdateType.Update;

    public float startGeschwindigkeit = 0.0f;
    public float lebenszeit = 15.0f;
    public float antriebsZeit = 3.0f;
    public float beschleunigung = 15.0f;
    public float drehung = 45.0f;

    LauncherSFX missileEffect;
    //Not needed anymore
    //private Vector3 launchVelocity = Vector3.zero;
    private float abschussZeit = 0.0f;
    private float aktivierungszeit = 0.0f;
    private float geschwindigkeit = 0.0f;
    private bool abgefeuert = false;
    private bool imFlug = false;
    private bool antrieb = false;

    private const float MINIMUM_GUIDE_SPEED = 1.0f;    // Verhindert das Marker kleiner Raketen zu groß werden. 


    public bool AntriebAktiv
    {
        get
        {
            return antrieb;
        }
    }

    private void Awake()
    {
        transform = GetComponent<Transform>();
        rigidbody = GetComponent<Rigidbody>();
        collider = GetComponent<CapsuleCollider>();
        missileEffect = GetComponent<LauncherSFX>();
    }

    private void Start()
    {
        // Verhindert, dass Rakete mit Spieler Schiff Kollidiert.
        if (spielerSchiff != null)
        {
            foreach (Collider col in spielerSchiff.GetComponentsInChildren<Collider>())
                Physics.IgnoreCollision(collider, col);
        }

        // Wenn Rakete noch nicht abgefeuert wurde, stelle sicher, dass sie "kinematic" ist.
        if (!abgefeuert)
            rigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (movementUpdateCycle == UpdateType.Update)
            Abschusssequenz();
    }

    private void FixedUpdate()
    {
        if (movementUpdateCycle == UpdateType.FixedUpdate)
            Abschusssequenz();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (abgefeuert && TimeSince(abschussZeit) > 0)    // Verhindert frühzeitige Explosion
        {
            DestroyMissile(true);
         }
    }

    // Rakete Abfeuern
    public void Abfeuern(Transform newTarget)
    {
        Abfeuern(newTarget, Vector3.zero);
    }

    // Rakete Abfeuern 
    public void Abfeuern(Transform newTarget, Vector3 inheritedVelocity)
    {
        if (!abgefeuert)
        {
            abgefeuert = true;
            abschussZeit = Time.time;            
            transform.parent = null;
            zielPunkt = newTarget;
           // launchVelocity = inheritedVelocity;
            rigidbody.isKinematic = false;
            AktivateRocket();
        }
    }

    private void Abschusssequenz()
    {
        if (abgefeuert)
        {
            // Aktiviert die Rakete erst, wenn abgefeuert wurde.
            if (!imFlug && TimeSince(abschussZeit) > 0.0f)    
                AktivateRocket();

            if (imFlug)
            {
                // Antrieb ist nur während Lebenszeit aktiv.
                if (antriebsZeit > 0.0f && TimeSince(aktivierungszeit) > antriebsZeit)
                    antrieb = false;
                else
                    antrieb = true;

                if (antrieb)
                    geschwindigkeit += beschleunigung * Time.deltaTime;

                // Rakete bewegen -> FixedUpdate ermöglicht zugriff auf rigidbody.velocity. 
                if (movementUpdateCycle == UpdateType.Update)
                    transform.Translate(transform.forward * geschwindigkeit * Time.deltaTime, Space.World);
                else if (movementUpdateCycle == UpdateType.FixedUpdate)
                    rigidbody.velocity = transform.forward * geschwindigkeit;
            }

            if (TimeSince(abschussZeit) > lebenszeit)
                DestroyMissile(false);
        }
    }

    private void AktivateRocket()
    {
        rigidbody.useGravity = false;
        rigidbody.velocity = Vector3.zero;
        imFlug = true;

        if (antriebsZeit <= 0.0f)  // Falls keine Antriebszeit angegeben, ist Antrieb immer aktiv
            antrieb = true;

        aktivierungszeit = Time.time;
        geschwindigkeit = startGeschwindigkeit;
    }

    private void DestroyMissile(bool impact)
    {
        Destroy(gameObject);

        if (missileEffect.SelfDestruct)
            missileEffect.Explode();    
        else if (impact)
            missileEffect.Explode();
    }

    private float TimeSince(float since)
    {
        return Time.time - since;
    }
}
