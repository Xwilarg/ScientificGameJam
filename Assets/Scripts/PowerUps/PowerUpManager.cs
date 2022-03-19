using System;
using ScientificGameJam.PowerUps;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEditorInternal;
using UnityEngine;

namespace ScientificGameJam.UI
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager instance { get; private set; }
        private List<PowerUp> powers;

        public List<PowerUp> GetPowerUps()
        {
            return powers;
        }

        public void AddPowerUp(PowerUp power)
        {
            powers.Add(power);
        }


        public void Awake()
        {
            if (instance != null)
            {
                Destroy(this.gameObject);
            }
            DontDestroyOnLoad(this.gameObject);
        }

        public static PowerUpManager getInstance()
        {
            if (instance == null)
            {
                instance = new GameObject("PowerUp Manager", typeof(PowerUpManager)).GetComponent<PowerUpManager>();
                DontDestroyOnLoad(instance.gameObject);
            }
            return instance;
        }
    }
}

