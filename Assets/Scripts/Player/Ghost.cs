using ScientificGameJam.SaveData;
using System.Collections.Generic;
using UnityEngine;

namespace ScientificGameJam.Player
{
    public class Ghost : MonoBehaviour
    {
        public void LoadData()
        {
            _rb = GetComponent<Rigidbody2D>();
            _coordinates = SaveLoad.Instance.Coordinates;
            _refTimer = Time.unscaledTime;
            _didStart = true;
        }

        private void FixedUpdate()
        {
            if (_didStart)
            {
                var targetTime = Time.unscaledTime - _refTimer;
                PlayerCoordinate last = _coordinates[0];
                for (int i = 1; i < _coordinates.Count; i++)
                {
                    var current = _coordinates[i];
                    if (targetTime >= last.TimeSinceStart && targetTime <= current.TimeSinceStart)
                    {
                        var prog = (targetTime - last.TimeSinceStart) / (current.TimeSinceStart - last.TimeSinceStart);
                        transform.position = Vector2.Lerp(last.Position, current.Position, prog);
                        _rb.velocity = Vector2.Lerp(last.Velocity, current.Velocity, prog);
                        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(last.Rotation, current.Rotation, prog));
                    }
                    last = current;
                }
            }
        }

        private Rigidbody2D _rb;

        private IReadOnlyList<PlayerCoordinate> _coordinates;
        private float _refTimer;
        private bool _didStart;
    }
}
