using ScientificGameJam.Debug;
using ScientificGameJam.PowerUp;
using ScientificGameJam.Race;
using ScientificGameJam.SaveData;
using ScientificGameJam.SO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace ScientificGameJam.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        [SerializeField]
        private GameObject _ghost;

        [SerializeField]
        private TMP_Text _timerCheckpointDiff;

        // Base controls
        private Rigidbody2D _rb;
        private float _verSpeed;
        private float _rot;

        // Saves and ghosts
        private readonly List<Ghost> _ghosts = new List<Ghost>();
        private readonly List<PlayerCoordinate> _currentCoordinates = new List<PlayerCoordinate>();
        private readonly List<float> _checkpointTimes = new List<float>();
        private float _timerRef;

        private float _speedBoost;

        private float _zoneModifier;
        public List<string> PassiveBoosts { private set; get; } = new List<string>();

        public void GainSpeedBoost(float percentage)
        {
            _speedBoost = percentage;
        }

        // Allow/Disallow player controls
        private bool _canMove;
        public bool CanMove
        {
            get => _canMove;
            set
            {
                if (!value)
                {
                    _rb.velocity = Vector3.zero;
                }
                _canMove = value;
            }
        }

        // Original pos and rot used for reset
        private Vector2 _orPos;
        private float _orRot;

        [SerializeField]
        private GameObject _powerupContainer;
        [SerializeField]
        private Image _powerupImage;

        public List<PowerupInfo> ActivePowerups { private set; get; } = new List<PowerupInfo>();

        private int _nextId;
        [SerializeField]
        private int _checkpointCount;

        [SerializeField]
        private int _remainingLapsRef;
        private int _remainingLaps;

        public void StopRace()
        {
            _zoneModifier = 1f;
            _speedBoost = 1f;
            _remainingLaps = _remainingLapsRef;
            _nextId = 0;
            transform.position = _orPos;
            transform.rotation = Quaternion.Euler(0f, 0f, _orRot);
            foreach (var ghost in _ghosts)
            {
                Destroy(ghost.gameObject);
            }
            _ghosts.Clear();
        }

        private void UpdatePowerupList()
        {
            if (ActivePowerups.Any())
            {
                _powerupContainer.SetActive(true);
                _powerupImage.sprite = ActivePowerups[0].Image;
            }
            else
            {
                _powerupContainer.SetActive(false);
            }
        }

        public void StartRace()
        {
            UpdatePowerupList();

            _currentCoordinates.Clear();
            _checkpointTimes.Clear();
            _nextCheckpointId = 0;
            _timerRef = Time.unscaledTime;
            CanMove = true;
            if (SaveLoad.Instance.HaveBestTime)
            {
                var go = Instantiate(_ghost, transform.position, transform.rotation);
                var ghost = go.GetComponent<Ghost>();
                ghost.LoadData();
                _ghosts.Add(ghost);
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _orPos = transform.position;
            _orRot = transform.rotation.eulerAngles.z;
        }

        private void Start()
        {
            StopRace();
        }

        private void Update()
        {
            if (_speedBoost > 1f)
            {
                _speedBoost -= Time.deltaTime * _info.BoostReduce;
                if (_speedBoost < 1f)
                {
                    _speedBoost = 1f;
                }
            }
        }

        private void FixedUpdate()
        {
            if (CanMove)
            {
                // If we are accelerating/descelerating
                if (Mathf.Abs(_verSpeed) > 0f)
                {
                    float speed = 0f;
                    if (_rb.velocity != Vector2.zero)
                    {
                        speed = _rb.velocity.magnitude * Vector2.Dot(_rb.velocity, transform.up) / Mathf.Abs(Vector2.Dot(_rb.velocity, transform.up));
                    }
                    _rb.velocity = transform.up.normalized * Mathf.Clamp(speed + _verSpeed, -_info.MaxSpeed, _info.MaxSpeed) * _speedBoost * _zoneModifier;
                }

                transform.Rotate(Vector3.back, _info.TorqueMultiplicator * _rot * _rb.velocity.magnitude);

                _currentCoordinates.Add(new PlayerCoordinate
                {
                    TimeSinceStart = Time.unscaledTime - _timerRef,
                    Position = transform.position,
                    Rotation = transform.rotation.eulerAngles.z,
                    Velocity = _rb.velocity
                });
            }

            if (DebugManager.Instance != null)
            {
                DebugManager.Instance.UpdateDebugText($"Speed: {_rb.velocity.magnitude:0.00}\nNext checkpoint: {_nextId}\nLaps remaining: {_remainingLaps}");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Player reached finish line
            if (collision.CompareTag("FinishLine") && _nextId == _checkpointCount)
            {
                if (_remainingLaps > 0)
                {
                    DisplayDelay();
                    _remainingLaps--;
                    _nextId = 0;
                }
                else
                {
                    _canMove = false; // Not using setter so we don't touch the rb
                    SaveLoad.Instance.UpdateBestTime(RaceManager.Instance.RaceTimer,
                        new List<PlayerCoordinate>(_currentCoordinates),
                        new List<float>(_checkpointTimes));
                    RaceManager.Instance.EndRace();
                }
            }
            else if (collision.CompareTag("Checkpoint") && _nextId == collision.gameObject.GetComponent<Checkpoint>().Id)
            {
                DisplayDelay();
                _nextId++;
            }
            else if (collision.CompareTag("ZoneBoost"))
            {
                var modifier = collision.gameObject.GetComponent<Modifier>();
                if (PassiveBoosts.Contains(modifier.TargetTag))
                {
                    _zoneModifier = modifier.SpeedModifierEnabled;
                }
                else
                {
                    _zoneModifier = modifier.SpeedModifierBase;
                }
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.CompareTag("ZoneBoost"))
            {
                _zoneModifier = 1f;
            }
        }

        private int _nextCheckpointId = 0;

        public void DisplayDelay()
        {
            var timer = Time.unscaledTime - _timerRef;
            _checkpointTimes.Add(timer);
            if (SaveLoad.Instance.HaveBestTime)
            {
                var diff = timer - SaveLoad.Instance.Checkpoints[_nextCheckpointId];
                _timerCheckpointDiff.gameObject.SetActive(true);
                _timerCheckpointDiff.text = (diff > 0 ? "+" : "") + diff.ToString("0.00");
                _timerCheckpointDiff.color = diff > 0 ? Color.red : Color.green;
                StartCoroutine(WaitAndDisappear());
            }
            _nextCheckpointId++;
        }

        private IEnumerator WaitAndDisappear()
        {
            yield return new WaitForSeconds(1f);
            _timerCheckpointDiff.gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Slow down player if he touch a wall
            _rb.velocity /= 1f;
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            var mov = value.ReadValue<Vector2>();
            _verSpeed = mov.y * _info.Acceleration;
            _rot = mov.x;
        }

        public void OnAction(InputAction.CallbackContext value)
        {
            if (value.performed && ActivePowerups.Any())
            {
                PowerUpManager.Instance.TriggerPowerup(ActivePowerups[0], this);
                ActivePowerups.RemoveAt(0);
                UpdatePowerupList();
            }
        }
    }
}