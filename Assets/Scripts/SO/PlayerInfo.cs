using UnityEngine;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Configuration")]

        [Tooltip("Speed of the player")]
        [Range(0f, 2f)]
        public float Acceleration = 1f;
        [Range(0f, 20f)]
        public float MaxSpeed;

        [Tooltip("Rotation speed")]
        [Range(0f, 1f)]
        public float TorqueMultiplicator = .5f;

        public float BoostReduce;
    }
}