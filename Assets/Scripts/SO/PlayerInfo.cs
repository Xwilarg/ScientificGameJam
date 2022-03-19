﻿using UnityEngine;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PlayerInfo", fileName = "PlayerInfo")]
    public class PlayerInfo : ScriptableObject
    {
        [Header("Configuration")]

        [Range(0f, 10f)]
        [Tooltip("Speed of the player")]
        public float Speed = 5f;
    }
}