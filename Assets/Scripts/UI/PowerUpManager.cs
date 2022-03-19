using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{
    private List<PowerUp> powers;

    public List<PowerUp> GetPowerUps()
    {
        return powers;
    }

    public void AddPowerUp(PowerUp power)
    {
        powers.Add(power);
    }

}
