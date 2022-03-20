using UnityEngine;

namespace ScientificGameJam.Player
{
    public class ChildFollow : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private Vector2 _orPos;
        private float _orRot;

        public Vector2 Offset;

        public bool IsInZone;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _orPos = transform.position;
            _orRot = transform.rotation.eulerAngles.z;
        }

        public void ResetAll()
        {
            transform.position = _orPos;
            transform.rotation = Quaternion.Euler(0f, 0f, _orRot);
            _rb.velocity = Vector2.zero;
        }

        public void Move(Vector2 vel, Vector2 pPos)
        {
            var dist = Vector2.Distance(pPos, transform.position);
            var mult = IsInZone ? 2.5f : 1f;
            if (dist > 5f * mult)
            {
                transform.position = pPos - Offset;
                _rb.velocity = vel;
            }
            else if (dist > 3f * mult)
            {
                _rb.velocity = vel * 2f;
            }
            else if (dist < 1.5f * mult)
            {
                _rb.velocity = vel / 2f;
            }
            else
            {
                _rb.velocity = vel;
            }
        }

        public void Rot(float rot)
        {
            transform.rotation = Quaternion.Euler(0f, 0f, rot);
        }
    }
}
