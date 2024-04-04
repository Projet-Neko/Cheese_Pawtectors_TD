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

    // Méthode pour activer l'effet "Aucune satiété pendant 60 secondes"
    public void ActivateNoSatietyEffect()
    {
        _powerUpType = PowerUpType.NoSatiety;
        StartCoroutine(ResetSatietyStateAfterDuration(60f)); // Réinitialiser l'état de la satiété après 60 secondes
    }

    private IEnumerator ResetSatietyStateAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        _powerUpType = PowerUpType.None;
    }

    // Méthode pour activer l'effet "Gain de viande doublé pendant 120 secondes"
    public void ActivateDoubleMeatEffect()
    {
        
    }

    // Méthode pour activer l'effet "Dégâts doublés pendant 120 secondes"
    public void ActivateDoubleDamageEffect()
    {
        
    }

    // Méthode pour activer l'effet "Vitesse d'attaque doublée pendant 60 secondes"
    public void ActivateDoubleAttackSpeedEffect()
    {
        
    }
}
