using System.Collections.Generic;
using UnityEngine;
using ScientificGameJam.SO;
using System;
using ScientificGameJam.Player;
using System.Linq;
using ScientificGameJam.UI;
using UnityEngine.UI;
using ScientificGameJam.Translation;

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
        [SerializeField]
        private GameObject _baseContainer;

        [Header("Parameters")]
        public int containerPadding;

        [Header("Lists")]
        [SerializeField] private PowerupInfo[] _powers;
        public List<PowerupInfo> AvailablePowerUps { private set; get; } = new List<PowerupInfo>();
        public PowerupInfo[] EquippedPowerUps { private set; get; } = new PowerupInfo[3];


        public void Awake()
        {
            Instance = this;

            // DEBUG
            foreach (var p in _powers)
            {
                AvailablePowerUps.Add(p);
            }
        }

        public void Start()
        {
            puPrefabHeight = puPrefab.GetComponent<RectTransform>().sizeDelta.y;

            ToggleDisplay(true);
        }

        public void GainPowerup()
        {
            var remainings = _powers.Where(x => !ContainsPowerup(x.Title)).ToArray();
            if (remainings.Any())
            {
                var random = remainings[UnityEngine.Random.Range(0, remainings.Length)];
                AvailablePowerUps.Add(random);
            }
        }

        private List<GameObject> _instanciated = new List<GameObject>();
        public void ToggleDisplay(bool status)
        {
            _baseContainer.SetActive(status);

            if (status)
            {
                foreach (var go in _instanciated)
                {
                    Destroy(go);
                }
                _instanciated.Clear();

                float yPos = -containerPadding;
                foreach (var power in AvailablePowerUps)
                {
                    GameObject pu = Instantiate(puPrefab, puContainer.transform);
                    ((RectTransform)pu.transform).anchoredPosition = new Vector2(0, yPos);

                    pu.GetComponent<Image>().sprite = power.Image;
                    pu.GetComponent<PUDragHandler>().powerUpName = power.Title;

                    _instanciated.Add(pu);

                    RectTransform rect = pu.GetComponent<RectTransform>();
                    rect.anchorMin = new Vector2(0.5f, 1f);
                    rect.anchorMax = new Vector2(0.5f, 1f);
                    rect.pivot = new Vector2(0.5f, 1f);

                    yPos -= containerPadding + puPrefabHeight;
                }
            }
        }

        public void AddPowerup(int index, string name)
        {
            EquippedPowerUps[index] = AvailablePowerUps.FirstOrDefault(x => x.Title == name);
        }

        public void RemovePowerup(int index)
        {
            EquippedPowerUps[index] = null;
        }

        public bool ContainsPowerup(string name)
        {
            return EquippedPowerUps.Any(x => x != null && x.Title == name);
        }

        public string GetPowerupDescription(string name)
        {
            var elem = _powers.First(x => x != null && x.Title == name);
            return $"{Translate.Instance.Tr(elem.DescriptionGame)}\n\n{Translate.Instance.Tr(elem.DescriptionScience)}";
        }

        public void ClearPowerups()
        {
            for (int i = 0; i < _powers.Length; i++)
            {
                EquippedPowerUps[i] = null;
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

