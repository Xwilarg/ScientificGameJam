using System.Collections.Generic;
using UnityEngine;
using ScientificGameJam.SO;
using System;

namespace ScientificGameJam.PowerUp
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }
        [SerializeField] private PowerupInfo[] _powers;
        public List<PowerupInfo> EquippedPowerUps { private set; get; } = new List<PowerupInfo>();

        public void Awake()
        {
            Instance = this;

            // Debug
            foreach (var power in _powers)
            {
                EquippedPowerUps.Add(power);
            }
        }

        public void TriggerPowerup(PowerupInfo info)
        {
            switch (info.Effect)
            {
                case PowerupEffect.DebugPrint:
                    UnityEngine.Debug.Log(info.Argument);
                    break;

                case PowerupEffect.SpeedBoost:
                    break;

                default:
                    throw new NotImplementedException($"{info.Effect} is not implemented");
            }
        }
    }
}

