using UnityEngine;

[DisallowMultipleComponent]
public class LauncherRemoveFX : MonoBehaviour
{
    ParticleSystem[] particles;
    public bool readyToDestroy = false;

    float effectStartTime = 0.0f;

    void OnEnable()
    {
        particles = GetComponentsInChildren<ParticleSystem>();
        effectStartTime = Time.time;
    }

    // Update mit jedem Frame
    void Update()
    {
        bool allParticlesCountZero = true;
        foreach (ParticleSystem ps in particles)
        {
            if (ps.particleCount > 0)
            {
                allParticlesCountZero = false;
                break;
            }
        }

        // Verhindert dass Effekte sich selbst zerstören vor dem start
        if (readyToDestroy && allParticlesCountZero && Time.time - effectStartTime > 1.0f)
            Destroy(gameObject);
    }
}
