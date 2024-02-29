using System;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

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


    public Slider _slider;

    // TODO -> add ui

    public virtual void TakeDamage(Entity source)
    {
        _currentHealth -= source.Damage;

        Mathf.Clamp(_currentHealth, 0f, _baseHealth);
        Debug.Log($"Current health after damages : {_currentHealth}");

        SetHealth();

        if (_currentHealth <= 0) Death(source);
    }

    protected virtual void Death(Entity source)
    {
        /* 
        Handle death logic
        - source : entity that killed (cat)
        - this : dying entity (mouse)
        */

        // Verify if "this" is a mouse
        if (this is not Mouse) return;
        if (source is Cat)
        {
            // Call the AddMeat function for the cat
            GameManager.Instance.AddMeat(1);
        }
        // When a mouse die add satiety to cat
        source.TakeDamage(this);
        OnDeath?.Invoke(this);
        Destroy(gameObject);
    }

    public void SetMaxHealth()
    {
        _slider.maxValue = _baseHealth;
        _slider.value = _baseHealth;
    }

    public void SetHealth()
    {
        _slider.value = _currentHealth;
    }
}