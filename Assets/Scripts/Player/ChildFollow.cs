using UnityEngine;

namespace ScientificGameJam.Player
{
    public class ChildFollow : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Vector2 _orPos;
        private float _orRot;

        private void Start()
        {
            _rb = GetComponent<Rigidbody2D>();
            _orPos = transform.position;
            _orRot = transform.rotation.eulerAngles.z;
        }

        public void ResetAll()
        {
            transform.position = _orPos;
            transform.rotation = Quaternion.Euler(0f, 0f, _orRot);
        }
    }
}
