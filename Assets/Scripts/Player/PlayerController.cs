using ScientificGameJam.Debug;
using ScientificGameJam.Race;
using ScientificGameJam.SaveData;
using ScientificGameJam.SO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace ScientificGameJam.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        [SerializeField]
        private GameObject _ghost;

        // Base controls
        private Rigidbody2D _rb;
        private float _verSpeed;
        private float _rot;

        // Saves and ghosts
        private readonly List<Ghost> _ghosts = new List<Ghost>();
        private readonly List<PlayerCoordinate> _currentCoordinates = new List<PlayerCoordinate>();
        private float _timerRef;

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

        public void StopRace()
        {
            transform.position = _orPos;
            transform.rotation = Quaternion.Euler(0f, 0f, _orRot);
            foreach (var ghost in _ghosts)
            {
                Destroy(ghost.gameObject);
            }
            _ghosts.Clear();
        }

        public void StartRace()
        {
            _currentCoordinates.Clear();
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
                    _rb.velocity = transform.up.normalized * Mathf.Clamp(speed + _verSpeed, -_info.MaxSpeed, _info.MaxSpeed);
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
                DebugManager.Instance.UpdateDebugText($"Speed: {_rb.velocity.magnitude:0.00}");
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Player reached finish line
            if (collision.CompareTag("FinishLine"))
            {
                _canMove = false; // Not using setter so we don't touch the rb
                RaceManager.Instance.EndRace();
                SaveLoad.Instance.UpdateBestTime(RaceManager.Instance.RaceTimer, new List<PlayerCoordinate>(_currentCoordinates));
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Slow down player if he touch a wall
            _rb.velocity /= 2f;
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            var mov = value.ReadValue<Vector2>();
            _verSpeed = mov.y * _info.Acceleration;
            _rot = mov.x;
        }
    }
}