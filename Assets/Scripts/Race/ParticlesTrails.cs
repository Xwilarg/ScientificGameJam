using System;
using System.Collections;
using System.Collections.Generic;
using ScientificGameJam.Player;
using UnityEngine;

public class ParticlesTrails : MonoBehaviour
{
    public int threshold = 10;

    private PlayerController _playerController;

    private void Start()
    {
        _playerController = FindObjectOfType<PlayerController>();
    }

    private void OnParticleTrigger()
    {
        ParticleSystem ps = GetComponent<ParticleSystem>();
            
        List<ParticleSystem.Particle> inside = new List<ParticleSystem.Particle>();
        // get
        int numInside = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);

        Debug.Log("Currently in : " + numInside);
        if (numInside > threshold)
        {
            _playerController.EnablePrintBonus();
        }
        else
        {
            _playerController.ResetPrintBonus();
        }
        
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Inside, inside);
            
    }
}
