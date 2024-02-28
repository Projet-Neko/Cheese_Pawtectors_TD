using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    public int WaveNumber => _waveNumber;

    private int _waveNumber;
    private readonly List<Mouse> _enemies = new();
    public Mouse mouse;
    private int MouseNBR;
    // TODO -> subscribe to cheese death event

    private Vector3 SpawnPos;

    public void Init()
    {
        _waveNumber = 0; // TODO -> get from database
        MouseNBR = 10;
        SpawnPos = transform.position;
        //SpawnPos.z = -3;
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
        if (_enemies.Count == 10) yield return null;

        Instantiate(mouse, SpawnPos, Quaternion.identity);
        _enemies.Add(new());
        yield return new WaitForSeconds(1);
    }

    public void NextWave()
    {
        //
    }
}