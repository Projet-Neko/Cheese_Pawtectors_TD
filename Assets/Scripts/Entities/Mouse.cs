using UnityEngine;

public class Mouse : Entity
{
    [Header("Debug")]
    [SerializeField] private bool _forceAlbino = false;
    //[SerializeField] private bool _forceBoss = false;

    //[SerializeField] private List<Vector3> _checkPoint;

    public Cat Attacker { get; set; }
    public bool IsBoss => _isBoss;
    public int WaveIndex { get; set; }

    private MouseSO _data;
    private bool _isBoss;

    /*private int _nextPoint;
    private float _distance;
    private Vector2 _destination;
    private Rigidbody2D _rb;*/

    public override void Init()
    {
        Attacker = null;
        _data = GameManager.Instance.Mouses[MouseType()];
        _level = GameManager.Instance.Data.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed;

        gameObject.name = $"{_data.Name} LVL{_level} [{WaveIndex}/{GameManager.Instance.MaxEnemyNumber}]";
        //Debug.Log($"Spawning {gameObject.name}");

        //_rb = GetComponent<Rigidbody2D>();
        //_nextPoint = 0;
        //_renderer.sprite = _data.Sprite;

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

    private void Move()
    {
        /*_distance = Vector2.Distance(transform.position, _checkPoint[_nextPoint]);

        _rb.velocity = _destination * _speed ;
        
        if (_distance < 0.05f)
        {
            _nextPoint++;

            if (_nextPoint == _checkPoint.Count) //arrivé au fromage 
            {
                _rb.velocity = new Vector2(0, 0);
                _stop = true;
                
                Attack();
            }
            else
            {
                _destination = (_checkPoint[_nextPoint] - transform.position);
                _destination.Normalize();
                _rb.velocity = _destination.normalized * _speed;

            }
        }*/
    }

    //public override void TakeDamage(Entity source)
    //{
    //    _targetedBy = source as Cat;
    //    base.TakeDamage(source);
    //}
}