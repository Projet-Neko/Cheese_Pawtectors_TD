using System;
using UnityEngine;
using UnityEngine.UI;

public abstract class Entity : MonoBehaviour
{
    public static event Action<Entity> OnDeath;

    [Header("Data")]
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected int _level = 1;

    [Header("HUD")]
    [SerializeField] protected Slider _slider;

    public float Damage => _damage;
    public float DPS => 3.6f - (_level * 0.1f);
    public float BaseHealth => _baseHealth;
    public float CurrentHealth => _currentHealth;
    public int Level => _level;
    public float Speed => _speed;
    public bool IsAttacked => _isAttacked;

    protected float _speed;
    protected float _baseHealth;
    protected float _currentHealth;
    protected float _damage;
    protected bool _isAttacked;

    private void Start()
    {
        Init();
    }

    public virtual void Init()
    {
        SetMaxHealth();
        SetHealth();
        _slider.gameObject.SetActive(false);
    }

    public virtual void TakeDamage(Entity source)
    {
        if (_currentHealth <= 0) return;
        _isAttacked = true;
        _slider.gameObject.SetActive(true);
        _currentHealth -= source.Damage;

        Mathf.Clamp(_currentHealth, 0f, _baseHealth);
        //Debug.Log($"Current health after damages : {_currentHealth}");

        SetHealth();

        if (_currentHealth <= 0) Die(source);
    }

    private void OnMouseOver()
    {
        _slider.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (this is Cat && _currentHealth != 0 || this is not Cat && _currentHealth != _baseHealth) return;
        _slider.gameObject.SetActive(false);
    }

    // Source => entity that killed
    public virtual void Die(Entity source)
    {
        if (source != null) OnDeath?.Invoke(this);
        if (this is not Mouse) return;
        if (source is Cat) source.TakeDamage(this); // When a mouse die add satiety to cat
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

    protected virtual void OnDeathEvent(Entity source) => OnDeath?.Invoke(source);
    public virtual bool IsAlive() => _currentHealth > 0;
}