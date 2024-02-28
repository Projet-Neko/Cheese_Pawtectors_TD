using UnityEngine;
using UnityEngine.UI;

public class Mouse : Entity
{
    private MouseSO _data;

    public int _maxHealth = 5;

    public Slider _slider;
    public Mouse _healthBar;

    private void Start()
    {
        _data = GameManager.Instance.Mouses[IsAlbino()];

        // TODO -> is queen if wave % 10

        _level = GameManager.Instance.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _healthBar.SetMaxHealth(_maxHealth);
        _damage = _data.SatiationRate;
        _speed = _data.Speed;

        //_renderer.sprite = _data.Sprite;

        gameObject.name = _data.Name;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(2);
        }
    }
    private int IsAlbino()
    {
        if (GameManager.Instance.CanSpawnAlbino && Random.Range(0, 100) <= 1)
        {
            GameManager.Instance.AlbinoHasSpawned();
            return 1;
        }

        return 0;
    }

    public void SetMaxHealth(int health)
    {
        _slider.maxValue = health;
        _slider.value = health;
    }

    public void SetHealth(int health)
    {
        _slider.value = health;
    }

    void TakeDamage(int damage)
    {
        _currentHealth -= damage;
    }
}