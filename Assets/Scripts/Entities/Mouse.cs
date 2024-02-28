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


    private void Start()
    {

        _data = GameManager.Instance.Mouses[IsAlbino()];

        // TODO -> is queen if wave % 10

        _level = GameManager.Instance.MouseLevel;

        _baseHealth = _currentHealth = _data.Health + (_level * 1) - 1;
        _damage = _data.SatiationRate;
        _speed = _data.Speed;
        _nextPoint = 1;

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
    private void FixedUpdate()
    {
        Move();
    }
    private void Move()
    {
        _distance = Vector2.Distance(transform.position, _checkPoint[_nextPoint]);
        _destination = _checkPoint[_nextPoint] - transform.position;


        if (_distance < 1f)
        {
            _nextPoint++;
            if (_nextPoint == _checkPoint.Count) { } //Attack Fromage;

        }

    }
}