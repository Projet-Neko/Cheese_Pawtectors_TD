using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    None,
    NoSatiety,
    DoubleMeat,
    DoubleDamage,
    DoubleAttackSpeed
}

public class Mod_PowerUp : Module
{
    private PowerUpType _powerUpType = PowerUpType.None;
    public PowerUpType PowerUpType => _powerUpType;

    // M�thode pour activer l'effet "Aucune sati�t� pendant 60 secondes"
    public void ActivateNoSatietyEffect()
    {
        _powerUpType = PowerUpType.NoSatiety;
        StartCoroutine(ResetSatietyStateAfterDuration(60f)); // R�initialiser l'�tat de la sati�t� apr�s 60 secondes
    }

    private IEnumerator ResetSatietyStateAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _powerUpType = PowerUpType.None;
    }

    // M�thode pour activer l'effet "Gain de viande doubl� pendant 120 secondes"
    public void ActivateDoubleMeatEffect()
    {
        
    }

    // M�thode pour activer l'effet "D�g�ts doubl�s pendant 120 secondes"
    public void ActivateDoubleDamageEffect()
    {
        
    }

    // M�thode pour activer l'effet "Vitesse d'attaque doubl�e pendant 60 secondes"
    public void ActivateDoubleAttackSpeedEffect()
    {
        
    }
}
