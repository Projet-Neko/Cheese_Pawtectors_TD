using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected SpriteRenderer _renderer;

    protected int _level;
    protected float _speed;
    protected float _baseHealth;
    protected float _currentHealth;
    protected int _damage;

    // TODO -> add ui

    public void TakeDamage(Entity source, int damage)
    {
        _currentHealth -= damage;

        Mathf.Clamp(_currentHealth, 0f, _baseHealth);

        if (_currentHealth == 0) Death(source);
    }

    protected virtual void LevelUp()
    {
        _level++;
    }

    protected virtual void Death(Entity source)
    {
        source.TakeDamage(this, _damage);
        Destroy(this);
    }
}