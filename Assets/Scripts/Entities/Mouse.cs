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
    private bool _arrived;


    private void Start()
    {

        _data = GameManager.Instance.Mouses[IsAlbino()];

        // TODO -> is queen if wave % 10

        _level = GameManager.Instance.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed;
        //_speed = 10;

        _rb = GetComponent<Rigidbody2D>();
        _nextPoint = 1;

        //_renderer.sprite = _data.Sprite;

        gameObject.name = _data.Name;

        _rb.velocity = _destination.normalized * _speed;
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
        if (!_arrived) Move();
        else { } //Attack 
    }
    private void Move()
    {
        if (!_arrived)
        {

            _distance = Vector2.Distance(transform.position, _checkPoint[_nextPoint]);
            _destination = _checkPoint[_nextPoint] - transform.position;

            _rb.velocity = _destination.normalized * (_speed+3); //---J'ai augmenté la vitesse // A ENLEVER

            if (_distance < 1f)
            {
                _nextPoint++;

                if (_nextPoint == _checkPoint.Count)
                {
                    _rb.velocity = new Vector2(0, 0);
                    _arrived = true;
                    //Attack Fromage;
                }
                else _rb.velocity = _destination.normalized * _speed;
            }
        }

    }
}