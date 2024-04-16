using System.Threading;
using UnityEngine;

public class Mouse : Entity
{
    [Header("Debug")]
    [SerializeField] private bool _forceAlbino = false;

    [SerializeField] GameObject _threat;

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

    private Vector3 _target;
    private Vector3 _direction;

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

    // Define the next coord that the mouse will go to
    public void DefineTarget(IdRoom target)
    {
        _target = new Vector3(target.x, transform.position.y, target.z);
        _direction = (_target - transform.position).normalized;
    }

    // Move the mouse to the target
    public void Move()
    {
        Vector3 movement = _direction * _speed * Time.deltaTime;

        if (movement.magnitude > Vector3.Distance(transform.position, _target))
            transform.position = _target;
        else
            transform.position += movement;
    }

    // Check if the mouse has reached the target
    public bool TargetReached()
    {
        return transform.position == _target;
    }

    //public override void TakeDamage(Entity source)
    //{
    //    _targetedBy = source as Cat;
    //    base.TakeDamage(source);
    //}

    protected override void DeathAnimation()
    {
        //Debug.Log("Cat is dead");
        Vector3 spawnPos = transform.position;
        for (int i = 0; i < Level; i++)
        {
            GameObject threat = _threat;
            Vector3 pos = new Vector3(spawnPos.x , spawnPos.y , spawnPos.z-5);
            Instantiate(threat, pos , Quaternion.Euler(90, 0, 0));
            threat.GetComponent<Rigidbody>().velocity = new Vector3(UnityEngine.Random.Range(-0.1f, 0.1f), UnityEngine.Random.Range(-0.1f, 0.1f), 0).normalized;
        }
    }
}