using NaughtyAttributes;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mod_Waves : Module
{
    public static event Action OnWaveReload;
    public static event Action BossWave;
    public static event Action OnBossDefeated;
    public static event Action WaveCompleted;
    public static event Action AlbinoSpawned;

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
        _spawningRoomPosition.y = 0.1f;
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

    private int MouseType()
    {
        if (GameManager.Instance.IsBossWave())// Check if it's a boss wave
        {
            // 3 = MouseBallBoss , 4 = RatBoss
            return UnityEngine.Random.Range(3, 5);
        }
        if (GameManager.Instance.CanSpawnBlackMouse)
        {
            GameManager.Instance.BlackMouseHasSpawned();
            // 2 = black mouse
            return 2;
        }
        if (GameManager.Instance.CanSpawnAlbino && UnityEngine.Random.Range(0, 100) <= 1)// Check if we can spawn Albinos
        {
            GameManager.Instance.AlbinoHasSpawned();
            AlbinoSpawned?.Invoke();
            // 1 = albino mouse
            return 1;
        }


        // 0 = classic mouse
        return 0;
    }

    public IEnumerator SpawnEnemies(bool cooldown)
    {
        _hasCompleteSpawning = false;
        _spawnedEnemyNumber = _killedEnemiesNumber = 0;
        if (IsBossWave())
        {
            BossWave?.Invoke();
            _maxEnemyNumber = 1;
        }
        else _maxEnemyNumber = 10;
        
        /*_maxEnemyNumber = 1;*/
        if (cooldown) yield return new WaitForSeconds(.5f);
        _cheeseDead = false;

        int index = 0;

        while (_spawnedEnemyNumber < _maxEnemyNumber)
        {
            //Mouse m = Instantiate(_gm.MousePrefab, _spawningRoomPosition, Quaternion.identity).GetComponent<Mouse>();
            Mouse m;
            int mouseType = MouseType();
            switch (mouseType)
            {
                case 2:
                    m = Instantiate(_gm.BlackMousePrefab, _spawningRoomPosition, Quaternion.identity).GetComponent<Mouse>();
                    break;
                case 3:
                    m = Instantiate(_gm.MouseBallPrefab, _spawningRoomPosition, Quaternion.identity).GetComponent<Mouse>();
                    break;
                case 4:
                    m = Instantiate(_gm.MouseRatPrefab, _spawningRoomPosition, Quaternion.identity).GetComponent<Mouse>();
                    break;
                default:
                    m = Instantiate(_gm.MousePrefab, _spawningRoomPosition, Quaternion.identity).GetComponent<Mouse>();
                    break;
            }
            m.transform.localEulerAngles = new Vector3(50, 35, 0);
            m.InitData(mouseType);
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