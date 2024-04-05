using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod_Waves : Module
{
    public static event Action OnWaveReload;

    [SerializeField] private GameObject _mousePrefab;
    [SerializeField] private int _spawnTime = 1;

    [Header("Options")]
    [SerializeField] private bool _enableWaves = false;

    //public int EnemyNumber => _enemyNumber;
    public int MaxEnemyNumber => _maxEnemyNumber;
    public int KilledEnemiesNumber => _killedEnemiesNumber;
    public int SpawnTime => _spawnTime;

    //private int _enemyNumber;
    private int _spawnedEnemyNumber;
    private int _maxEnemyNumber;
    private int _killedEnemiesNumber;
    private List<GameObject> _enemyObjects = new();
    private Vector3 _SpawnPos;
    private bool _hasCompleteSpawning;
    private IEnumerator _spawn;
    private bool _cheeseDead = false;

    private void Awake()
    {
        Cheese.OnInit += Cheese_OnInit;
        Entity.OnDeath += Entity_OnDeath;
    }

    private void Cheese_OnInit(Cheese obj)
    {
        StartWaves();
    }

    private void OnDestroy()
    {
        Cheese.OnInit -= Cheese_OnInit;
        Entity.OnDeath -= Entity_OnDeath;
    }

    private void Update()
    {
        if (_hasCompleteSpawning && _killedEnemiesNumber == _maxEnemyNumber) NextWave();
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _SpawnPos = transform.position;
        _SpawnPos.z = -4;
        _hasCompleteSpawning = false;
        InitComplete();
    }

    public void StartWaves()
    {
        _maxEnemyNumber = _killedEnemiesNumber = 0;
        _spawn = SpawnEnemies(false);
        if (_enableWaves) StartCoroutine(_spawn);
    }

    public IEnumerator SpawnEnemies(bool cooldown)
    {
        int index = 0;
        _hasCompleteSpawning = false;
        _maxEnemyNumber = IsBossWave() ? 1 : 10;
        if (cooldown) yield return new WaitForSeconds(.5f);
        while (_spawnedEnemyNumber < _maxEnemyNumber)
        {
            Mouse m = Instantiate(_mousePrefab, _SpawnPos, Quaternion.identity).GetComponent<Mouse>();
            m.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            m.gameObject.name = $"{m.gameObject.name} #{index}";
            _spawnedEnemyNumber++;
            index++;
            _enemyObjects.Add(m.gameObject);
            yield return new WaitForSeconds(1);
        }

        _hasCompleteSpawning = true;
        yield return null;
    }

    private void Entity_OnDeath(Entity obj, bool hasBeenKilledByPlayer)
    {
        if (obj is Cheese)
        {
            _cheeseDead = true;
            Reload();
        }
        else if (obj is Mouse && !_cheeseDead)
        {
            _killedEnemiesNumber++;
            _spawnedEnemyNumber--;
        }
    }

    public bool IsBossWave()
    {
        // Check if the wave number is a multiple of 10
        return _gm.Data.WaveNumber % 10 == 0;
    }

    public void NextWave()
    {
        _gm.Data.UpdateWaves();
        Debug.Log($"Next wave : {_gm.Data.WaveNumber}.");

        Reload();
    }

    public void Reload()
    {
        Debug.Log("Reloading wave...");

        if (!_hasCompleteSpawning) StopCoroutine(_spawn);

        if (_spawnedEnemyNumber != 0)
        {
            Debug.Log($"Destroying remaining {_spawnedEnemyNumber} enemies...");

            foreach (var enemy in _enemyObjects) if (enemy != null) Destroy(enemy);
            _enemyObjects.Clear();
        }

        OnWaveReload?.Invoke();
        _spawnedEnemyNumber = _killedEnemiesNumber = 0;
        _cheeseDead = false;
        _spawn = SpawnEnemies(true);
        StartCoroutine(_spawn);
    }
}