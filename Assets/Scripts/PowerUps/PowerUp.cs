using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ScientificGameJam.PowerUps
{
    public abstract class PowerUp : MonoBehaviour
    {
        //Name and description
        public string title;
        public string description;

        //Image to display
        public Image image;

        //Wether the power-up is passive or activates with a button
        public bool passive;
    }
}

