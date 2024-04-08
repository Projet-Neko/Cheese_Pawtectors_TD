using UnityEngine;

public class Mouse : Entity
{
    [Header("Debug")]
    [SerializeField] private bool _forceAlbino = false;

    //[SerializeField] private bool _forceBoss = false;

    //[SerializeField] private List<Vector3> _checkPoint;

    public Cat Attacker { get; set; }
    public bool IsBoss => _isBoss;


    private MouseSO _data;
    private bool _isBoss;

    /*private int _nextPoint;
    private float _distance;
    private Vector2 _destination;
    private Rigidbody2D _rb;*/

    private bool _hasEaten = false;
    private Vector3 _target;
    private Vector3 _direction;

    public bool HasEaten => _hasEaten;

    public override void Init()
    {
        Attacker = null;
        _data = GameManager.Instance.Mouses[MouseType()];


        _level = GameManager.Instance.Data.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed;

        //_rb = GetComponent<Rigidbody2D>();
        //_nextPoint = 0;
        //_renderer.sprite = _data.Sprite;

        //gameObject.name = _data.Name;

        /*_destination = (_checkPoint[_nextPoint] - transform.position);
        _destination.Normalize();
        _rb.velocity = _destination.normalized * _speed;*/
        base.Init();
    }

    private int IsAlbino()
    {
        if (_forceAlbino || (GameManager.Instance.CanSpawnAlbino && Random.Range(0, 100) <= 1))
        {
            GameManager.Instance.AlbinoHasSpawned();
            // 1 = albino mouse
            return 1;
        }

        return 0;
    }

    private int MouseType()
    {
        if (GameManager.Instance.IsBossWave())
        {
            _isBoss = true;
            // 3 = boss
            return 3;
        }
        else
        {
            return IsAlbino();
        }
    }

    public void DefineTarget(IdRoom target)
    {
        _speed = 1; // TO DO : change this
        _target = new Vector3(target.x, target.y, transform.position.z);
        _direction = (_target - transform.position).normalized;
    }

    public void Move()
    {
        Vector3 movement = _direction * _speed * Time.deltaTime;

        if (movement.magnitude > Vector3.Distance(transform.position, _target))
            transform.position = _target;
        else
            transform.position += movement;
    }

    public bool TargetReached()
    {
        return transform.position == _target;
    }

    public void Eat()
    {
        _hasEaten = true;
    }



    //public override void TakeDamage(Entity source)
    //{
    //    _targetedBy = source as Cat;
    //    base.TakeDamage(source);
    //}
}