using System;

public class Cheese : Entity
{
    public static event Action CheeseDeath;


    private void Awake()
    {
        _baseHealth = _currentHealth = 3;
    }

    protected override void Death(Entity source)
    {
        // TODO -> event game over
        
        CheeseDeath?.Invoke();

        //Destroy effect ?
        Restart();
    }

    public override void TakeDamage(Entity source) 
    {
        _currentHealth--;
        SetHealth();
        if (_currentHealth <= 0)
        {
            CheeseDeath?.Invoke();
            Restart() ;
        }
    }

    private void Restart()
    {
        _currentHealth = _baseHealth;
    }
}