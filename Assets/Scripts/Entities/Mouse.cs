using Unity.VisualScripting;
using UnityEngine;
using System.Collections.Generic;


public class Mouse : Entity
{
    [SerializeField] private List<Vector3> _checkPoint;

    private MouseSO _data;

    private int _nextPoint;
    private float _distance;
    private Vector3 _destination;
    private Rigidbody2D _rb;
    private bool _stop;


    private void Start()
    {

        _data = GameManager.Instance.Mouses[IsAlbino()];

        // TODO -> is queen if wave % 10

        _level = GameManager.Instance.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed * 3;
        //_speed = 10;

        _rb = GetComponent<Rigidbody2D>();
        _nextPoint = 0;

        //_renderer.sprite = _data.Sprite;

        gameObject.name = _data.Name;

        _destination = (_checkPoint[_nextPoint] - transform.position).normalized;
        _rb.velocity = _destination * _speed;
        _stop = false;
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
    private void FixedUpdate()
    {
        if (!_stop) Move();
        else { } //Attack 
    }
    private void Move()
    {
        _speed = _data.Speed * 3; //---J'ai augmenté la vitesse // A ENLEVER

        _distance = Vector2.Distance(transform.position, _checkPoint[_nextPoint]);

        _rb.velocity = _destination * _speed ; 

        if (_distance < 0.05f)
        {
            _nextPoint++;
            _destination = (_checkPoint[_nextPoint] - transform.position).normalized;

            if (_nextPoint == _checkPoint.Count)
            {
                _rb.velocity = new Vector2(0, 0);
                _stop = true;
                //Attack Fromage;
            }
            else _rb.velocity = _destination * _speed;
        }

    }

    public void Stop()
    {
        _stop = true;
    }

}