using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Wave : MonoBehaviour
{
    public static event Action OnWaveReload;

    [SerializeField] private GameObject _mousePrefab;

    public int WaveNumber => _waveNumber;

    private int _waveNumber;
    private readonly List<Mouse> _enemies = new();
    private Vector3 _SpawnPos;
    private bool _hasCompleteSpawning;

    private IEnumerator _spawn;

    private void Awake()
    {
        Entity.OnDeath += Entity_OnDeath;
    }

    private void OnDestroy()
    {
        Entity.OnDeath -= Entity_OnDeath;
    }

    private void Update()
    {
        if (_hasCompleteSpawning && _enemies.Count == 0) NextWave();
    }

    public void Init()
    {
        _waveNumber = 0; // TODO -> get from database
        _SpawnPos = transform.position;
        _hasCompleteSpawning = false;
    }

    public void StartWave()
    {
        _spawn = SpawnEnemies(false);
        StartCoroutine(_spawn);
    }

    public IEnumerator SpawnEnemies(bool cooldown)
    {
        _hasCompleteSpawning = false;

        if (cooldown) yield return new WaitForSeconds(.5f);

        while (_enemies.Count < 10)
        {
            yield return new WaitForSeconds(1);
            Mouse m = Instantiate(_mousePrefab, _SpawnPos, Quaternion.identity).GetComponent<Mouse>();
            _enemies.Add(m);
        }

        _hasCompleteSpawning = true;
        yield return null;
    }

    private void Entity_OnDeath(Entity obj)
    {
        if (obj is Cheese)
        {
            Reload();
            return;
        }

        if (obj is Mouse)
        {
            // TODO -> remove enemy from list on death
        }
    }

    public void NextWave()
    {
        _waveNumber++;
        Debug.Log($"Next wave : {_waveNumber}.");

        Reload();
        // TODO -> level up mouses in M_Entities
    }

    public void Reload()
    {
        Debug.Log("Reloading wave...");

        if (!_hasCompleteSpawning) StopCoroutine(_spawn);

        if (_enemies.Count != 0)
        {
            Debug.Log("Destroying remaining enemies...");

            foreach (var enemy in _enemies) Destroy(enemy.gameObject);
            _enemies.Clear();
        }

        OnWaveReload?.Invoke();
        _spawn = SpawnEnemies(true);
        StartCoroutine(_spawn);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, 1);
    }
}