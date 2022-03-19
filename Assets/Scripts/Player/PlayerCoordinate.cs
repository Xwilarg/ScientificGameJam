using UnityEngine;

namespace ScientificGameJam.Player
{
    /// <summary>
    /// Set of coordinate of the player
    /// </summary>
    public class PlayerCoordinate
    {
        public float Rotation { set; get; }
        public Vector2 Position { set; get; }
        public float TimeSinceStart { set; get; }
        public Vector2 Velocity { set; get; }
    }
}
