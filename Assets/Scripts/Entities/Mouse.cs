public class Mouse : Entity
{
    private MouseSO _data;

    private void Awake()
    {
        _data = GameManager.Instance.Mouses[0];

        // TODO -> random for albino mouse, queen if wave%10

        _level = GameManager.Instance.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed;

        //_renderer.sprite = _data.Sprite;
    }
}