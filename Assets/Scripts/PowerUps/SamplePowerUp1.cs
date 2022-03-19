using System.Collections;
using System.Collections.Generic;
using ScientificGameJam.PowerUps;
using ScientificGameJam.UI;
using UnityEngine;

public class SamplePowerUp1 : PowerUp
{
    SamplePowerUp1()
    {
        title = "Polite PowerUp";
        description = "Prints `Hello` when the game starts";
        passive = true;
    }
    
    public override void doEffect()
    {
        Debug.Log("Hello");
    }
}
