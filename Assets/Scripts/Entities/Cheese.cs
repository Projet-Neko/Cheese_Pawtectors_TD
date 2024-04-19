using System;

public class Cheese : Entity
{
    public static event Action<Cheese> OnInit;

    private void Awake()
    {
        Mod_Waves.OnWaveReload += OnWaveReload;
    }

    private void OnWaveReload()
    {
        _currentHealth = 30;
        base.Init();
    }

    public override void Init()
    {
        _baseHealth = _currentHealth = 30;
        OnInit?.Invoke(this);
        base.Init();
    }
}