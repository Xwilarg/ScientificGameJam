using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesTrails : MonoBehaviour
{
    public int threshold = 10;
    private void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
            
        List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
        // get
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        if (numInside > threshold)
        {
            
        }
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
            
    }
}
