using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerUpType
{
    None,
    Satiety,
    DoubleMeat,
    DoubleDamage,
    DoubleAttackSpeed
}

public class PowerUpManager : MonoBehaviour
{
    // Méthode pour activer l'effet "Aucune satiété pendant 60 secondes"
    public void ActivateNoSatietyEffect()
    {
        
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

    public void ActivatePowerUpEffect(PowerUpType powerUpType)
    {
        switch (powerUpType)
        {
            case PowerUpType.Satiety:
                ActivateNoSatietyEffect();
                break;
            case PowerUpType.DoubleMeat:
                ActivateDoubleMeatEffect();
                break;
            case PowerUpType.DoubleDamage:
                ActivateDoubleDamageEffect();
                break;
            case PowerUpType.DoubleAttackSpeed:
                ActivateDoubleAttackSpeedEffect();
                break;
            default:
                Debug.LogWarning("Unknown power-up type: " + powerUpType);
                break;
        }
    }
}
