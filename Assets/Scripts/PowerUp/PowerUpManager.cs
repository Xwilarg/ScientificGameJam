using System.Collections.Generic;
using UnityEngine;
using ScientificGameJam.SO;
using System;

namespace ScientificGameJam.PowerUp
{
    public class PowerUpManager : MonoBehaviour
    {
        public static PowerUpManager Instance { get; private set; }

        [Header("Dependancies")]
        [Tooltip("The container with the list of UI_PowerUps")]
        public GameObject puContainer;

        [Tooltip("Prefab of the UI_PowerUp")]
        public GameObject puPrefab;
        private float puPrefabHeight;

        [Header("Parameters")]
        public int containerPadding;

        [Header("Lists")]
        [SerializeField] private PowerupInfo[] _powers;
        public List<PowerupInfo> AvailablePowerUps { private set; get; } = new List<PowerupInfo>();
        public List<PowerupInfo> EquippedPowerUps { private set; get; } = new List<PowerupInfo>();


        public void Awake()
        {
            Instance = this;

            // Debug
            foreach (var power in _powers)
            {
                AvailablePowerUps.Add(power);
            }
        }

        public void Start()
        {
            float yPos = -containerPadding;
            puPrefabHeight = puPrefab.GetComponent<RectTransform>().sizeDelta.y;

            foreach (var power in AvailablePowerUps)
            {
                GameObject pu = Instantiate(puPrefab, puContainer.transform);
                pu.transform.localPosition = new Vector2(0, yPos);

                RectTransform rect = pu.GetComponent<RectTransform>();
                rect.anchorMin = new Vector2(0.5f, 1f);
                rect.anchorMax = new Vector2(0.5f, 1f);
                rect.pivot = new Vector2(0.5f, 1f);

                yPos -= containerPadding + puPrefabHeight;
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

