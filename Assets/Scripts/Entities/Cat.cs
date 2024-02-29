using UnityEngine;

public class Cat : Entity
{
    [SerializeField] CatBrain _brain;

    public Color CatColor => _catColor;
    public bool IsSleeping => _currentSatiety == _baseSatiety;
    public float DPS => 3.6f - (_level * 0.1f);

    private Color _catColor;
    private float _baseSatiety;
    private float _currentSatiety;
    private bool _isInStorageMode;

    private CatSO _data;

    private void Start()
    {
        _speed = 1;
        Init(_level);
    }

    public void Init(int level)
    {
        _level = level;
        _damage = level; // TODO -> change formula
        _data = GameManager.Instance.Cats[_level - 1];
        _catColor = ColorPalette.GetColor(_data.Color);
        _baseSatiety = 50 + (_level * 2) - 2;
        _currentSatiety = 0;
        _isInStorageMode = false;

        //_renderer.sprite = _data.SpriteAbove;
        // TODO -> check sprite to use

        // Waiting for sprites
        _renderer.color = CatColor;
        Debug.Log(CatColor);

        gameObject.name = _data.Name;
    }

    public void SetStorageMode(bool mode)
    {
        _isInStorageMode = mode;
        if (_isInStorageMode) _brain.ChangeState(new SStorage());
    }

    public void LevelUp()
    {
        _level++;
        Init(_level);
        Debug.Log($"Level up to lvl {_level}");
    }

    public override void TakeDamage(Entity source)
    {
        _currentSatiety += source.Damage;

        Mathf.Clamp(_currentSatiety, 0f, _baseSatiety);
        Debug.Log($"Cat current satiety : {_currentSatiety}/{_baseSatiety}");

        if (_currentSatiety == _baseSatiety) Sleep();
    }

    private void Sleep()
    {
        //
    }
}