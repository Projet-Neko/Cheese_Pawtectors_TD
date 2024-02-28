using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int WaveNumber => _waveNumber;

    private int _waveNumber;
    private readonly List<Mouse> _enemies = new();

    // TODO -> subscribe to cheese death event

    public void Init()
    {
        _waveNumber = 0; // TODO -> get from database
    }

    public void StartWave()
    {
        StartCoroutine(SpawnEnemies());
    }

    public IEnumerator SpawnEnemies()
    {
        if (_enemies.Count == 10) yield return null;
        _enemies.Add(new());
        yield return new WaitForSeconds(1);
    }

    public void NextWave()
    {
        //
    }
}