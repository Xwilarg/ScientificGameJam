using ScientificGameJam.Debug;
using ScientificGameJam.SaveData;
using ScientificGameJam.SO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ScientificGameJam.Player
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private PlayerInfo _info;

        private Rigidbody2D _rb;
        private float _verSpeed;
        private float _rot;

        private readonly List<PlayerCoordinate> _currentCoordinates = new List<PlayerCoordinate>();
        private float _timerRef;

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
                else // Race started
                {
                    _timerRef = Time.unscaledTime;
                }
                _canMove = value;
            }
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!CanMove)
            {
                return;
            }

            // If we are accelerating/descelerating
            if (Mathf.Abs(_verSpeed) > 0f)
            {
                var speed = _rb.velocity.magnitude;
                _rb.velocity = transform.up.normalized * Mathf.Clamp(speed + _verSpeed, -_info.MaxSpeed, _info.MaxSpeed);
            }

            transform.Rotate(Vector3.back, _info.TorqueMultiplicator * _rot * _rb.velocity.magnitude);

            if (DebugManager.Instance != null)
            {
                DebugManager.Instance.UpdateDebugText($"Speed: {_rb.velocity.magnitude:0.00}");
            }

            _currentCoordinates.Add(new PlayerCoordinate
            {
                TimeSinceStart = Time.unscaledTime - _timerRef,
                Position = transform.position,
                Rotation = transform.rotation.eulerAngles.z
            });
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Player reached finish line
            if (collision.CompareTag("FinishLine"))
            {
                _canMove = false;
                RaceManager.Instance.EndRace();
                SaveLoad.Instance.UpdateBestTime(RaceManager.Instance.RaceTimer, _currentCoordinates);
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