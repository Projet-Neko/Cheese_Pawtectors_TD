using System;
using System.Threading;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public enum CatState
{
    Lock, Unlock
}

public class Cat : Entity
{
    public static event Action<int> OnUnlock;

    [SerializeField] TMP_Text _catLevel;

    [Header("Debug Only")]
    [SerializeField] Sprite _sprite;

    public Color CatColor => _catColor;
    public bool IsInStorageMode => _isInStorageMode;
    public Sprite[] Sprites => _data.Sprites;

    private CatSO _data;
    private Color _catColor;
    private bool _isInStorageMode = true;

    public override void Init()
    {
        Init(_level);
        base.Init();
    }

    public void Init(int level)
    {
        // Update data with SO
        _data = GameManager.Instance.Cats[level - 1];

        _level = level;
        _damage = _data.Damage();
        _dps = _data.DPS();
        _catLevel.text = _level.ToString();

        _baseHealth = _data.Satiety();
        _currentHealth = 0;
        _speed = _data.Speed();

        // Default Appearance
        _renderer.sprite = _data.Sprites[4];

        gameObject.name = _data.Name;
    }

    public void SetStorageMode(bool mode)
    {
        int size = mode ? 55 : 30;
        _isInStorageMode = mode;
        transform.localScale = new Vector3(size, size, size);
        if (!_isInStorageMode) return;

        // Default Appearance
        _renderer.sprite = _data.Sprites[4];

        _slider.gameObject.SetActive(false);
    }

    public void LevelUp()
    {
        _level++;
        Init(_level);
        //Debug.Log($"Level up to lvl {_level}");

        if (_data.State == CatState.Lock) OnUnlock?.Invoke(_level - 1);
    }

    public override void TakeDamage(Entity source)
    {

        if (GameManager.Instance.IsPowerUpActive(PowerUpType.NoSatiety))
        {
            //Debug.Log($"Cat current satiety(noSatiety) : {_currentHealth}/{_baseHealth}");
            return;
        }
        else
        {
            _currentHealth += source.Damage;

            Mathf.Clamp(_currentHealth, 0f, _baseHealth);
            //Debug.Log($"Cat current satiety : {_currentHealth}/{_baseHealth}");

            SetHealth();

            if (_currentHealth == _baseHealth) Sleep();
        }
    }

    private void Sleep()
    {
        // TODO -> add animation
    }

    public void WakeUp()
    {
        _currentHealth = 0;
        SetHealth();
    }

    public override bool IsAlive() => _currentHealth < _baseHealth;

    
}