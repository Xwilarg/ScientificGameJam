using System.Collections.Generic;
using UnityEngine;
using ScientificGameJam.SO;
using System;
using ScientificGameJam.Player;
using System.Linq;

namespace ScientificGameJam.PowerUp
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }
        [SerializeField] private PowerupInfo[] _powers;
        public List<PowerupInfo> AvailablePowerUps { private set; get; } = new List<PowerupInfo>();
        public PowerupInfo[] EquippedPowerUps { private set; get; } = new PowerupInfo[3];

        public void Awake()
        {
            Instance = this;

            // Debug
            foreach (var power in _powers)
            {
                AvailablePowerUps.Add(power);
            }
            EquippedPowerUps[0] = AvailablePowerUps[0];
        }

        public void AddPowerup(int index, string name)
        {
            _powers[index] = AvailablePowerUps.FirstOrDefault(x => x.Title == name);
        }

        public void RemovePowerup(int index)
        {
            _powers[index] = null;
        }

        public bool ContainsPowerup(PowerupInfo info)
        {
            return _powers.Any(x => x.Title == info.Title);
        }

        public void ClearPowerups()
        {
            for (int i = 0; i < _powers.Length; i++)
            {
                _powers[i] = null;
            }
        }

        public void TriggerPowerup(PowerupInfo info, PlayerController player)
        {
            switch (info.Effect)
            {
                case PowerupEffect.DebugPrint:
                    UnityEngine.Debug.Log(info.Argument);
                    break;

                case PowerupEffect.SpeedBoost:
                    player.GainSpeedBoost(float.Parse(info.Argument));
                    break;

                default:
                    throw new NotImplementedException($"{info.Effect} is not implemented");
            }
        }
    }
}

