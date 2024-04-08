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
    // List of single elements
    private HashSet<PowerUpType> activePowerUps = new HashSet<PowerUpType>();

    public HashSet<PowerUpType> ActivePowerUps => activePowerUps;

    // Dictionary associating each type of power-up with its duration
    private Dictionary<PowerUpType, float> powerUpDurations = new Dictionary<PowerUpType, float>()
    {
        { PowerUpType.NoSatiety, 60f },
        { PowerUpType.DoubleMeat, 120f },
        { PowerUpType.DoubleDamage, 120f },
        { PowerUpType.DoubleAttackSpeed, 60f }
    };

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        InitComplete();
    }

    private float GetDuration(PowerUpType powerUpType)
    {
        return powerUpDurations[powerUpType];
    }

    public bool IsPowerUpActive(PowerUpType powerUpType)
    {
        return activePowerUps.Contains(powerUpType);
    }

    public void ActivatePowerUp(PowerUpType powerUpType)
    {
        activePowerUps.Add(powerUpType);
        float duration = GetDuration(powerUpType);
        StartCoroutine(ResetPowerUp(powerUpType, duration));
    }

    private IEnumerator ResetPowerUp(PowerUpType powerUpType, float duration)
    {
        yield return new WaitForSeconds(duration);
        activePowerUps.Remove(powerUpType);
    }
}