using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    NoSatiety,
    DoubleMeat,
    DoubleDamage,
    DoubleAttackSpeed
}


public class Mod_PowerUp : Module
{

    private PowerUpType _powerUpType = PowerUpType.None;
    public PowerUpType PowerUpType => _powerUpType;

    private HashSet<PowerUpType> activePowerUps = new HashSet<PowerUpType>();

    public bool IsPowerUpActive(PowerUpType powerUpType)
    {
        return activePowerUps.Contains(powerUpType);
    }

    public void ActivatePowerUp(PowerUpType powerUpType, float duration)
    {
        activePowerUps.Add(powerUpType);
        StartCoroutine(ResetPowerUp(powerUpType, duration));
    }

    private IEnumerator ResetPowerUp(PowerUpType powerUpType, float duration)
    {
        yield return new WaitForSeconds(duration);
        activePowerUps.Remove(powerUpType);
    }
}
