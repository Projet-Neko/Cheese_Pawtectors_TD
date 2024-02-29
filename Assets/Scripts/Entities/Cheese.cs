using System;

public class Cheese : Entity
{
    public static event Action<Cheese> OnInit;

    private void Awake()
    {
        M_Wave.OnWaveReload += OnWaveReload;
    }

    private void OnWaveReload()
    {
        _currentHealth = _baseHealth;
    }

    private void Start()
    {
        _baseHealth = _currentHealth = 3;
        OnInit?.Invoke(this);
    }

    public override void TakeDamage(Entity source) 
    {
        if (_currentHealth <= 0) return;
        _currentHealth--;

        //SetHealth();

        if (_currentHealth <= 0) OnDeathEvent(this);
    }
}