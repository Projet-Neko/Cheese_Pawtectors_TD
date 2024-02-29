using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    [SerializeField] private Mouse _mouse;
    [SerializeField] private Cheese _cheese;

    public int WaveNumber => _waveNumber;

    private int _waveNumber;
    private readonly List<Mouse> _enemies = new();
    

    // TODO -> subscribe to cheese death event

    private Vector3 _SpawnPos;
    
    public void Init()
    {
        _waveNumber = 0; // TODO -> get from database
        _SpawnPos = transform.position;
        
    }

    private void Start()
    {
        Init();
        StartWave();
    }

    public void StartWave()
    {

        StartCoroutine(SpawnEnemies());

    }

    public IEnumerator SpawnEnemies()
    {

        while (_enemies.Count < 10)
        {
            yield return new WaitForSeconds(1);
           CreateMouse();

        }
    }

    private void CreateMouse()
    {
        Instantiate(_mouse, _SpawnPos, Quaternion.identity);
        _enemies.Add(_mouse);
        _mouse.SetCheese(_cheese);
    }

    public void NextWave()
    {
        _waveNumber++;
    }

    public void ReloadWave()
    {
        StopCoroutine(SpawnEnemies());
        foreach (var enemy in _enemies) Destroy(enemy);

    }
}