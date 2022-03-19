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

namespace ScientificGameJam.Race
{
    public class RaceManager : MonoBehaviour
    {
        public static RaceManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

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

        public float RaceTimer { private set; get; }
        private bool _didRaceStart;

        private void Start()
        {
            StartRace();
        }

        private void Update()
        {
            if (_didRaceStart)
            {
                RaceTimer += Time.deltaTime;
                _mainTimer.text = $"{RaceTimer:0.00}";
            }
        }

        public void StartRace()
        {
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
            if (RaceTimer < _currentLevel.Medals[0].Time)
            {
                _msgEnd.text = Translate.Instance.Tr("gotAllMedals");
            }
            else
            {
                foreach (var medal in _currentLevel.Medals.Reverse())
                {
                    if (RaceTimer > medal.Time)
                    {
                        _msgEnd.text = Translate.Instance.Tr("nextMedal", $"{medal.Time:0.00}");
                        break;
                    }
                }
            }
            PowerUpManager.Instance.EquippedPowerUps.Clear();
        }

        private IEnumerator LaunchRaceCountdown()
        {
            _didRaceStart = false;
            _raceCountdown.text = "3";
            yield return new WaitForSeconds(1f);
            _raceCountdown.text = "2";
            yield return new WaitForSeconds(1f);
            _raceCountdown.text = "1";
            yield return new WaitForSeconds(1f);
            _raceCountdown.gameObject.SetActive(false);

            _player.ActivePowerups.Clear();
            foreach (var power in PowerUpManager.Instance.EquippedPowerUps)
            {
                if (power.IsPassive)
                {
                    PowerUpManager.Instance.TriggerPowerup(power, _player);
                }
                else
                {
                    _player.ActivePowerups.Add(power);
                }
            }

            _player.StartRace();
            _didRaceStart = true;
        }

        public void OnRestart(InputAction.CallbackContext input)
        {
            if (input.performed)
            {
                _player.StopRace();
                StartRace();
            }
        }
    }
}