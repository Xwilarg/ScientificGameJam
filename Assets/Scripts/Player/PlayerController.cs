using ScientificGameJam.Debug;
using ScientificGameJam.SO;
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

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            // If we are accelerating/descelerating
            if (Mathf.Abs(_verSpeed) > 0f)
            {
                _rb.velocity = _info.SpeedMultiplicator * _verSpeed * transform.up;
            }

            transform.Rotate(Vector3.back, _info.TorqueMultiplicator * _rot * _rb.velocity.magnitude);

            DebugManager.Instance.UpdateDebugText($"Speed: {_rb.velocity.magnitude:0.00}");
        }

        public void OnMovement(InputAction.CallbackContext value)
        {
            var mov = value.ReadValue<Vector2>();
            UnityEngine.Debug.Log(mov);
            _verSpeed = mov.y;
            _rot = mov.x;
        }
    }
}