using UnityEngine;

public class Cat : Entity
{
    public Color CatColor => ColorPalette.GetColor(_data.Color);
    public float DPS => 3.6f - (_data.Level * 0.1f);

    private CatSO _data;

    private void Awake()
    {
        _speed = 1;
    }

    public void Init(int level)
    {
        _level = _damage = level;
        _data = GameManager.Instance.Cats[_level - 1];
        _baseHealth = _currentHealth = 50 + (_level * 2) - 2;

        //_renderer.sprite = _data.SpriteAbove;
        // TODO -> check sprite to use

        // Waiting for sprites
        _renderer.color = CatColor;

        gameObject.name = _data.Name;
    }

    public void LevelUp()
    {
        _level++;
        Init(_level);
    }

    protected override void Death(Entity source)
    {
        // TODO -> State mode repos
    }
}