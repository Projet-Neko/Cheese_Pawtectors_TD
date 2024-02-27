using UnityEngine;

public class Mouse : Entity
{
    private MouseSO _data;

    private void Start()
    {
        _data = GameManager.Instance.Mouses[IsAlbino()];

        // TODO -> is queen if wave % 10

        _level = GameManager.Instance.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed;

        //_renderer.sprite = _data.Sprite;

        gameObject.name = _data.Name;
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
}