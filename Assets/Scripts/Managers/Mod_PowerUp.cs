using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    NoSatiety = 60,
    DoubleMeat = 120,
    DoubleDamage = 120,
    DoubleAttackSpeed = 60
}


public class Mod_PowerUp : Module
{
    // List of single elements
    private HashSet<PowerUpType> activePowerUps = new HashSet<PowerUpType>();

    public HashSet<PowerUpType> ActivePowerUps => activePowerUps;

    public bool IsPowerUpActive(PowerUpType powerUpType)
    {
        return activePowerUps.Contains(powerUpType);
    }

    public void ActivatePowerUp(PowerUpType powerUpType)
    {
        activePowerUps.Add(powerUpType);
        float duration = (float)powerUpType;
        StartCoroutine(ResetPowerUp(powerUpType, duration));
    }

    private IEnumerator ResetPowerUp(PowerUpType powerUpType, float duration)
    {
        yield return new WaitForSeconds(duration);
        activePowerUps.Remove(powerUpType);
    }
}
