using TMPro;
using UnityEngine;

public class Cat : Entity
{
    [SerializeField] TMP_Text _catLevel;

    [Header("Debug Only")]
    [SerializeField] Sprite _sprite;

    public Color CatColor => _catColor;
    public bool IsInStorageMode => _isInStorageMode;

    private CatSO _data;
    private Color _catColor;
    private bool _isInStorageMode;

    private void Awake()
    {
        _isInStorageMode = false;
    }

    public override void Init()
    {
        Init(_level);
        base.Init();
    }

    public void Init(int level)
    {
        _level = level;
        _damage = level; // TODO -> change formula
        _catLevel.text = _level.ToString();

        _baseHealth = 50 + (_level * 2) - 2;
        _currentHealth = 0;
        _speed = 5;

        // Update data with SO
        _data = GameManager.Instance.Cats[_level - 1];

        // Appearance
        _catColor = ColorPalette.GetColor(_data.Color);
        _renderer.sprite = _data.SpriteAbove; // TODO -> check sprite to use
        if (_data.SpriteAbove == null) _renderer.color = CatColor;

        gameObject.name = _data.Name;
    }

    public void SetStorageMode(bool mode)
    {
        _isInStorageMode = mode;
        if (!_isInStorageMode) return;
        _slider.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        _level++;
        Init(_level);
        //Debug.Log($"Level up to lvl {_level}");
    }

    public override void TakeDamage(Entity source)
    {
        _currentHealth += source.Damage;

        Mathf.Clamp(_currentHealth, 0f, _baseHealth);
        Debug.Log($"Cat current satiety : {_currentHealth}/{_baseHealth}");

        SetHealth();

        if (_currentHealth == _baseHealth) Sleep();
    }

    private void Sleep()
    {
        // TODO -> add animation
    }

    public void WakeUp()
    {
        _currentHealth = 0;
    }

    public override bool IsAlive() => _currentHealth < _baseHealth;
}