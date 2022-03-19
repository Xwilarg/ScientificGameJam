using UnityEngine;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/LevelInfo", fileName = "LevelInfo")]
    public class LevelInfo : ScriptableObject
    {
        public string SceneName;
        public float Gold, Silver, Bronze;
    }
}