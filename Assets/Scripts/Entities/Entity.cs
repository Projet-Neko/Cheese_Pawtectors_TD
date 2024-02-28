using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    public static event Action<Entity> OnDeath;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected int _level = 1;

    public float Damage => _damage;
    public float BaseHealth => _baseHealth;
    public float CurrentHealth => _currentHealth;
    public int Level => _level;

    public float _speed;
    protected float _baseHealth;
    protected float _currentHealth;
    protected float _damage;

    // TODO -> add ui

    public virtual void TakeDamage(Entity source)
    {
        _currentHealth -= source.Damage;

        Mathf.Clamp(_currentHealth, 0f, _baseHealth);
        Debug.Log($"Current health after damages : {_currentHealth}");

        if (_currentHealth == 0) Death(source);
    }

    protected virtual void Death(Entity source)
    {
        if (this is not Mouse) return;
        source.TakeDamage(this);
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }
}