using ScientificGameJam.Audio;
using ScientificGameJam.Player;
using ScientificGameJam.PowerUp;
using ScientificGameJam.SaveData;
using ScientificGameJam.SO;
using ScientificGameJam.Translation;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScientificGameJam.Race
{
    public class RaceManager : MonoBehaviour
    {
        public static RaceManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
            _source = GetComponent<AudioSource>();
        }

        [SerializeField]
        private Arrow[] _arrows;

        [SerializeField]
        private LevelInfo _currentLevel;

        /// <summary>
        /// Text displaying the countdown when the race start
        /// </summary>
        [SerializeField]
        private TMP_Text _raceCountdown;

        /// <summary>
        /// Current and best time of the player in the race
        /// </summary>
        [SerializeField]
        private TMP_Text _mainTimer, _bestTimer;

        /// <summary>
        /// Reference to the player
        /// </summary>
        [SerializeField]
        private PlayerController _player;

        [SerializeField]
        private Camera _playerCamera, _overviewCamera;

        [Header("Course end")]
        [SerializeField]
        private GameObject _courseEndGo;

        [SerializeField]
        private TMP_Text _timerEnd, _msgEnd;

        [SerializeField]
        private Image[] _medals;

        private AudioSource _source;

        [SerializeField]
        private AudioClip _bipLow, _bipHigh;

        public float RaceTimer { private set; get; }
        private bool _didRaceStart;

        private void Update()
        {
            if (_didRaceStart)
            {
                RaceTimer += Time.deltaTime;
                _mainTimer.text = $"{RaceTimer:0.00}";
            }
        }

        public void Stop()
        {
            _player.StopRace();
        }

        public void HideView()
        {
            _courseEndGo.SetActive(false);
            _playerCamera.gameObject.SetActive(false);
            _overviewCamera.gameObject.SetActive(true);

            BGMManager.Instance.PlayPowerupSelect();
        }

        public void StartRace()
        {
            BGMManager.Instance.PlayDuringRace();

            _courseEndGo.SetActive(false);

            // Set camera on player
            _playerCamera.gameObject.SetActive(true);
            _overviewCamera.gameObject.SetActive(false);

            // Reset timer and text
            RaceTimer = 0f;
            _mainTimer.text = "0.00";
            _bestTimer.text = $"{Translate.Instance.Tr("best")}{Translate.Instance.Tr("colon")} ";
            if (SaveLoad.Instance.HaveBestTime)
            {
                _bestTimer.text += $"{SaveLoad.Instance.BestTime:0.00}";
            }
            else
            {
                _bestTimer.text += $"{Translate.Instance.Tr("none")}";
            }

            // Prevent player to move until countdown ends
            _player.CanMove = false;
            _raceCountdown.gameObject.SetActive(true);

            StartCoroutine(LaunchRaceCountdown());
        }

        public void EndRace()
        {
            _didRaceStart = false;
            _courseEndGo.SetActive(true);
            _timerEnd.text = $"{RaceTimer:0.00}s";
            foreach (var m in _medals)
            {
                m.color = Color.black;
            }
            if (SaveLoad.Instance.BestTime < _currentLevel.Medals[0].Time)
            {
                _msgEnd.text = Translate.Instance.Tr("gotAllMedals");
                foreach (var m in _medals)
                {
                    m.color = Color.white;
                }
            }
            else
            {
                var _currMedals = _currentLevel.Medals.Reverse().ToArray();
                for (int i = 0; i < _currMedals.Length; i++)
                {
                    if (SaveLoad.Instance.BestTime > _currMedals[i].Time)
                    {
                        _msgEnd.text = Translate.Instance.Tr("nextMedal", $"{_currMedals[i].Time:0.00}");
                        break;
                    }
                    _medals[i].color = Color.white;
                }
            }
            PowerUpManager.Instance.ClearPowerups();
            foreach (var a in _arrows)
            {
                a.DisplayNormal();
            }
        }

        private IEnumerator LaunchRaceCountdown()
        {
            _didRaceStart = false;
            _raceCountdown.text = "3";
            _source.PlayOneShot(_bipLow);
            yield return new WaitForSeconds(1f);
            _raceCountdown.text = "2";
            _source.PlayOneShot(_bipLow);
            yield return new WaitForSeconds(1f);
            _raceCountdown.text = "1";
            _source.PlayOneShot(_bipLow);
            yield return new WaitForSeconds(1f);
            _source.PlayOneShot(_bipHigh);
            _raceCountdown.gameObject.SetActive(false);

            RefreshPowerups();

            _player.StartRace();
            _didRaceStart = true;
        }

        private void RefreshPowerups()
        {
            _player.ActivePowerups.Clear();
            _player.PassiveBoosts.Clear();
            foreach (var power in PowerUpManager.Instance.EquippedPowerUps)
            {
                if (power == null)
                {
                    continue;
                }
                if (power.IsPassive)
                {
                    // PowerUpManager.Instance.TriggerPowerup(power, _player);
                    if (power.Effect == PowerupEffect.ZoneBoost)
                    {
                        _player.PassiveBoosts.Add(power.Argument);
                    }
                }
                else
                {
                    _player.ActivePowerups.Add(power);
                }
            }
            var haveArrowPowerup = PowerUpManager.Instance.EquippedPowerUps.Any(x => x != null && x.Id == "durotaxieName");
            foreach (var a in _arrows)
            {
                if (haveArrowPowerup)
                {
                    a.DisplayNormal();
                }
                else
                {
                    a.DisplayGrey();
                }
            }
        }

        public void OnRestart(InputAction.CallbackContext input)
        {
            if (input.performed)
            {
                _player.StopRace();
                RefreshPowerups();
                StartRace();
            }
        }
    }
}