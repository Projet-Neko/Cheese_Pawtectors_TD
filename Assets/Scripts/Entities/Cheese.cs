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

    protected override void Start()
    {
        _baseHealth = _currentHealth = 30;
        OnInit?.Invoke(this);
        base.Start();
    }
}