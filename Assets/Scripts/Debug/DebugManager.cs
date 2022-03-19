using TMPro;
using UnityEngine;

namespace ScientificGameJam.Debug
{
    public class DebugManager : MonoBehaviour
    {
        public static DebugManager Instance { private set; get; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private TMP_Text _debugText;

        public void UpdateDebugText(string text)
        {
            if (_debugText != null)
            {
                _debugText.text = text;
            }
        }
    }
}
