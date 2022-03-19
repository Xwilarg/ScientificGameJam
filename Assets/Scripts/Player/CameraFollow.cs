using UnityEngine;

namespace ScientificGameJam.Player
{
    public class CameraFollow : MonoBehaviour
    {
        [SerializeField]
        private Transform _target;

        private Vector2 _offset;
        private float _baseZ;

        private void Start()
        {
            _baseZ = transform.position.z;
            _offset = transform.position - _target.position;
        }

        private void Update()
        {
            var newOffset = (Vector3)_offset + _target.position;
            transform.position = new Vector3(newOffset.x, newOffset.y, _baseZ);
        }
    }
}
