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
    public static event Action WaveCompleted;

    [SerializeField, Scene] private string _buildScene;
    [SerializeField, Scene] private string _mainScreenScene;

    [Header("Options")]
    [SerializeField] private int _spawnTime = 1;
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
    private bool _hasCompleteSpawning = false;
    private IEnumerator _spawn;
    private bool _cheeseDead;
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

    private void OnDestroy()
    {
        Cheese.OnInit -= Cheese_OnInit;
        StartRoom.OnInit -= StartRoom_OnInit;
        Entity.OnDeath -= Entity_OnDeath;
        SceneManager.sceneLoaded -= SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Additive) return;

        if (scene.name == _buildScene)
        {
            Debug.Log("Waves disabled");
            _enableWaves = false;
            _wavesStarted = false;
            StopCoroutine(_spawn);
        }
        else if (scene.name == _mainScreenScene)
        {
            Debug.Log("Waves enabled");
            _enableWaves = true;
        }
    }

    private void StartRoom_OnInit(Room obj)
    {
        _spawningRoomPosition = obj.transform.position;
    }

    private void Cheese_OnInit(Cheese obj)
    {
        _cheeseInitialized = true;
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        InitComplete();
    }

    private void Update()
    {
        if (_enableWaves && !_wavesStarted && _cheeseInitialized && _spawningRoomPosition != new Vector3(-1, -1, -1))
        {
            StartWaves();
        }
    }

    public void StartWaves()
    {
        _wavesStarted = true;
        _maxEnemyNumber = 0;
        _spawn = SpawnEnemies(false);
        if (_enableWaves) StartCoroutine(_spawn);
    }

    public IEnumerator SpawnEnemies(bool cooldown)
    {
        _hasCompleteSpawning = false;
        _spawnedEnemyNumber = _killedEnemiesNumber = 0;
        /*_maxEnemyNumber = IsBossWave() ? 1 : 10;*/
        _maxEnemyNumber = 1;
        if (cooldown) yield return new WaitForSeconds(.5f);
        _cheeseDead = false;

        int index = 0;

        while (_spawnedEnemyNumber < _maxEnemyNumber)
        {
            Mouse m = Instantiate(_gm.MousePrefab, _spawningRoomPosition, Quaternion.identity).GetComponent<Mouse>();
            m.WaveIndex = index + 1;
            _spawnedEnemyNumber++;
            _enemyObjects.Add(m.gameObject);
            index++;
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
            //Debug.Log($"{_killedEnemiesNumber} mouse killed");

            if ((entity as Mouse).IsBoss) OnBossDefeated?.Invoke();
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
        WaveCompleted?.Invoke();
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

        foreach (var enemy in _enemyObjects) if (enemy != null) Destroy(enemy);
        _enemyObjects.Clear();

        OnWaveReload?.Invoke();
        _spawn = SpawnEnemies(true);
        StartCoroutine(_spawn);
    }
}