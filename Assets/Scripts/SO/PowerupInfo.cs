using ScientificGameJam.PowerUp;
using UnityEngine;
using UnityEngine.UI;

namespace ScientificGameJam.SO
{
    [CreateAssetMenu(menuName = "ScriptableObject/PowerupInfo", fileName = "PowerupInfo")]
    public class PowerupInfo : ScriptableObject
    {
        public string Title;
        public string Description;

        public Image Image;

        //Wether the power-up is passive or activates with a button
        public bool IsPassive;

        [Tooltip("Argument given to execution method")]
        public string Argument;

        public PowerupEffect Effect;
    }
}