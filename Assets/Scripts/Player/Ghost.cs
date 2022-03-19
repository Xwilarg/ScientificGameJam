using ScientificGameJam.SaveData;
using System.Collections.Generic;
using UnityEngine;

namespace ScientificGameJam.Player
{
    public class Ghost : MonoBehaviour
    {
        public void LoadData()
        {
            _coordinates = SaveLoad.Instance.Coordinates;
            _refTimer = Time.unscaledTime;
            _didStart = true;
        }

        private void Update()
        {
            if (_didStart)
            {
                var targetTime = Time.unscaledTime - _refTimer;
                PlayerCoordinate last = null;
                for (int i = 0; i < _coordinates.Count; i++)
                {
                    var current = _coordinates[i];
                    if (current.TimeSinceStart < targetTime)
                    {
                        if (last == null)
                        {
                            last = current;
                        }
                        var prog = (last.TimeSinceStart - targetTime) / (current.TimeSinceStart - targetTime);
                        transform.position = Vector2.Lerp(last.Position, current.Position, prog);
                        transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(last.Rotation, current.Rotation, prog));
                    }
                    last = current;
                }
            }
        }

        private IReadOnlyList<PlayerCoordinate> _coordinates;
        private float _refTimer;
        private bool _didStart;
    }
}
