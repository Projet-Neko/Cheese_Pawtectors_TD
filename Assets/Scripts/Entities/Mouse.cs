using UnityEngine;
using System.Collections.Generic;

public class Mouse : Entity
{
    //[SerializeField] private List<Vector3> _checkPoint;
    [SerializeField] private bool _forceAlbino = false;

    private MouseSO _data;

    /*private int _nextPoint;
    private float _distance;
    private Vector2 _destination;
    private Rigidbody2D _rb;*/
    private bool _stop;
    private Cheese _cheese;

    public override void Init()
    {
        _data = GameManager.Instance.Mouses[IsAlbino()];

        // TODO -> is queen if wave % 10

        _level = GameManager.Instance.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed;

        //_rb = GetComponent<Rigidbody2D>();
        //_nextPoint = 0;

        
        _currentHealth = 5;
        //_renderer.sprite = _data.Sprite;
        _cheese = GameManager.Instance.Cheese;

        gameObject.name = _data.Name;

        /*_destination = (_checkPoint[_nextPoint] - transform.position);
        _destination.Normalize();
        _rb.velocity = _destination.normalized * _speed;*/
        _stop = false;
        base.Init();
    }

    private int IsAlbino()
    {
        if (_forceAlbino || (GameManager.Instance.CanSpawnAlbino && Random.Range(0, 100) <= 1))
        {
            GameManager.Instance.AlbinoHasSpawned();
            return 1;
        }

        return 0;
    }

    private void FixedUpdate()
    {
        if (!_stop) Move();
       
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

    public void Stop()
    {
        _stop = true;
    }

    private void Attack()
    {
        _cheese?.TakeDamage(this);
    }
}