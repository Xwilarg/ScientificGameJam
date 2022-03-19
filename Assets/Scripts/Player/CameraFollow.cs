using UnityEngine;

namespace ScientificGameJam.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private Vector2 _offset;

        private void Start()
        {
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            transform.position = (Vector3)_offset + _target.position;
        }
    }
}
