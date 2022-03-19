using UnityEngine;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Configuration")]

        [Range(0f, 10f)]
        [Tooltip("Speed of the player")]
        public float SpeedMultiplicator = 5f;

        [Tooltip("Rotation speed")]
        public float TorqueMultiplicator = .5f;
    }
}