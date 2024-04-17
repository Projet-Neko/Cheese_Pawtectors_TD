using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mod_Waves : Module
{
    public static event Action OnWaveReload;
    public static event Action OnBossDefeated;

    [SerializeField] private GameObject _mousePrefab;
    [SerializeField] private int _spawnTime = 1;

    [Header("Options")]
    [SerializeField] private bool _enableWaves = false;

    [Header("Scenes")]
    [SerializeField, Scene] private string _sceneBuild;
    [SerializeField, Scene] private string _sceneMain;


    //public int EnemyNumber => _enemyNumber;
    public int MaxEnemyNumber => _maxEnemyNumber;
    public int KilledEnemiesNumber => _killedEnemiesNumber;
    public int SpawnTime => _spawnTime;

    //private int _enemyNumber;
    private int _spawnedEnemyNumber;
    private int _maxEnemyNumber;
    private int _killedEnemiesNumber;
    private List<GameObject> _enemyObjects = new();
    private bool _hasCompleteSpawning;
    private IEnumerator _spawn;
    private bool _cheeseDead = false;
    private Vector3 _spawningRoomPosition = new(-1, -1, -1);
    private bool _wavesStarted = false;
    private bool _cheeseInitialized = false;

    private void Awake()
    {
        Cheese.OnInit += Cheese_OnInit;
        StartRoom.OnInit += StartRoom_OnInit;
        Entity.OnDeath += Entity_OnDeath;
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void StartRoom_OnInit(Room obj)
    {
        _spawningRoomPosition = obj.transform.position;
        //_spawningRoomPosition.z = -4; // Necessary ?
        PrepareWaves();
    }

    private void Cheese_OnInit(Cheese obj)
    {
        _cheeseInitialized = true;
        PrepareWaves();
    }

    private void OnDestroy()
    {
        Cheese.OnInit -= Cheese_OnInit;
        StartRoom.OnInit -= StartRoom_OnInit;
        Entity.OnDeath -= Entity_OnDeath;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _hasCompleteSpawning = false;
        InitComplete();
    }

    // Check if all conditions are met to start the waves
    private void PrepareWaves()
    {
        if (_enableWaves && !_wavesStarted && _cheeseInitialized && _spawningRoomPosition != new Vector3(-1, -1, -1))
            StartWaves();
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == _sceneBuild)
            DisableWaves();
        else if (scene.name == _sceneMain)
            EnableWaves();
    }

    private void DisableWaves()
    {
        _enableWaves = false;
        _wavesStarted = false;
        Reload();
    }

    private void EnableWaves()
    {
        _enableWaves = true;
        PrepareWaves();
    }

    public void StartWaves()
    {
        _wavesStarted = true;
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
            Mouse m = Instantiate(_mousePrefab, _spawningRoomPosition, Quaternion.identity).GetComponent<Mouse>();
            m.WaveIndex = index + 1;
            _spawnedEnemyNumber++;
            index++;
            _enemyObjects.Add(m.gameObject);
            yield return new WaitForSeconds(1);
        }

        _hasCompleteSpawning = true;
        yield return null;
    }

    private void Entity_OnDeath(Entity entity, bool hasBeenKilledByPlayer)
    {
        if (entity is Cheese)
        {
            _cheeseDead = true;
            GameOver();
        }
        else if (entity is Mouse && !_cheeseDead)
        {
            _killedEnemiesNumber++;
            _spawnedEnemyNumber--;

            if (entity is Mouse m && m.IsBoss)
            {
                OnBossDefeated?.Invoke();
            }
            if (_hasCompleteSpawning && _killedEnemiesNumber == _maxEnemyNumber) NextWave();

        }
    }

    public bool IsBossWave()
    {
        // Check if the wave number is a multiple of 10
        return _gm.Data.WaveNumber % 10 == 0;
    }

    public void NextWave()
    {
        _gm.Data.UpdateWaves(true);
        Debug.Log($"Next wave : {_gm.Data.WaveNumber}.");

        Reload();
    }

    private void GameOver()
    {
        _gm.Data.UpdateWaves(false, IsBossWave());
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