using TMPro;
using UnityEngine;

public class Cat : Entity
{
    [SerializeField] CatBrain _brain;
    [SerializeField] TMP_Text _catLevel;

    public Color CatColor => _catColor;

    private Color _catColor;
    private bool _isInStorageMode;

    private CatSO _data;

    protected override void Start()
    {
        _speed = 5;
        Init(_level);
        _isInStorageMode = false;
        base.Start();
    }

    public void Init(int level)
    {
        _level = level;
        _damage = level; // TODO -> change formula
        _data = GameManager.Instance.Cats[_level - 1];
        _catColor = ColorPalette.GetColor(_data.Color);
        _baseHealth = 50 + (_level * 2) - 2;
        _currentHealth = 0;

        gameObject.name = _data.Name;

        _renderer.sprite = _data.SpriteAbove;
        // TODO -> check sprite to use

        // Waiting for sprite
        if (_data.SpriteAbove == null) _renderer.color = CatColor;
    }

    public void SetStorageMode(bool mode)
    {
        _isInStorageMode = mode;
        if (!_isInStorageMode) return;
        _brain.ChangeState(new SStorage());
        _slider.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        _level++;
        _catLevel.text = _level.ToString();
        Init(_level);
        Debug.Log($"Level up to lvl {_level}");
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
        //
    }

    public override bool IsAlive()
    {
        return _currentHealth < _baseHealth;
    }
}