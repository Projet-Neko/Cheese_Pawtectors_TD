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
    // M�thode pour activer l'effet "Aucune sati�t� pendant 60 secondes"
    public void ActivateNoSatietyEffect()
    {
        
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
