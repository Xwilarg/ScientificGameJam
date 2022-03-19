using UnityEngine;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Configuration")]

        [Range(0f, 20f)]
        [Tooltip("Speed of the player")]
        public float SpeedMultiplicator = 5f;

        [Tooltip("Rotation speed")]
        [Range(0f, 1f)]
        public float TorqueMultiplicator = .5f;
    }
}