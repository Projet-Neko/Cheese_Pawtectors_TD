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
        _damage = _data.SatiationRate;
        _speed = _data.Speed;
        _currentHealth = 5;
        //_renderer.sprite = _data.Sprite;
        _healthBar.SetMaxHealth();

        gameObject.name = _data.Name;
    }

    void Update()
    {
        Entity mouse = this;

        if (Input.GetKeyDown(KeyCode.I))
        {
            mouse.TakeDamage(_healthBar);
            SetHealth();
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

    public void SetMaxHealth()
    {
        _slider.maxValue = _maxHealth;
        _slider.value = _maxHealth;
    }

    public void SetHealth()
    {
        _slider.value = _currentHealth;
    }
}