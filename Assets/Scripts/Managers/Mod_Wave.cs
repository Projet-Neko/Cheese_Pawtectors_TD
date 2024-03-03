using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mod_Wave : Mod
{
    public static event Action OnWaveReload;

    [SerializeField] private GameObject _mousePrefab;

    [Header("Options")]
    [SerializeField] private bool _enableWaves = false;

    public int WaveNumber => _waveNumber;

    private int _waveNumber;
    private int _enemyNumber;
    private List<GameObject> _enemyObjects = new();
    private Vector3 _SpawnPos;
    private bool _hasCompleteSpawning;
    private IEnumerator _spawn;

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
        if (_hasCompleteSpawning && _enemyNumber == 0) NextWave();
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _waveNumber = 0; // TODO -> get from database
        _SpawnPos = transform.position;
        _SpawnPos.z = -4;
        _hasCompleteSpawning = false;
    }

    public void StartWaves()
    {
        _enemyNumber = 0;
        _spawn = SpawnEnemies(false);
        if (_enableWaves) StartCoroutine(_spawn);
    }

    public IEnumerator SpawnEnemies(bool cooldown)
    {
        int index = 0;
        _hasCompleteSpawning = false;

        if (cooldown) yield return new WaitForSeconds(.5f);

        while (_enemyNumber < 10)
        {
            Mouse m = Instantiate(_mousePrefab, _SpawnPos, Quaternion.identity).GetComponent<Mouse>();
            m.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            m.gameObject.name = $"{m.gameObject.name} #{index}";
            _enemyNumber++;
            index++;
            _enemyObjects.Add(m.gameObject);
            yield return new WaitForSeconds(1);
        }

        _hasCompleteSpawning = true;
        yield return null;
    }

    private void Entity_OnDeath(Entity obj)
    {
        if (obj is Cheese) Reload();
        else if (obj is Mouse) _enemyNumber--;
    }

    public void NextWave()
    {
        _waveNumber++;
        Debug.Log($"Next wave : {_waveNumber}.");

        Reload();
    }

    public void Reload()
    {
        Debug.Log("Reloading wave...");

        if (!_hasCompleteSpawning) StopCoroutine(_spawn);

        if (_enemyNumber != 0)
        {
            Debug.Log($"Destroying remaining {_enemyNumber} enemies...");

            foreach (var enemy in _enemyObjects) if (enemy != null) Destroy(enemy);
            _enemyObjects.Clear();
        }

        OnWaveReload?.Invoke();
        _enemyNumber = 0;
        _spawn = SpawnEnemies(true);
        StartCoroutine(_spawn);
    }
}