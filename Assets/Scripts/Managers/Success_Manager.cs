using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Success_Manager : MonoBehaviour
{

    private string successFolderPath = "SO/Success/"; // Le chemin du dossier où se trouvent les ScriptableObject des succès
    private List<SuccessSO> allSuccesses = new List<SuccessSO>(); 

    private Dictionary<string, int> _success = new Dictionary<string, int>();

    static public event Action SuccessEvent;
    static public event Action<List<SuccessSO>> DisplaySuccessEvent;

    private int _successCount = 0;
    private int _mouseKilled = 0;
    private int _bossKilled = 0;
    private int _wavesDone = 0;
    private int _catAdopted = 0;
    private int _pieceCount = 0;

    private void Awake()
    {
        SceneManager.sceneLoaded += DisplaySuccess;
        Entity.OnDeath += CheckDeathEvent;
        //Mod_Waves.OnBossDefeated += BossKilled;
        //Mod_Waves.WaveCompleted += CompleteWaves;
        //Cat.OnUnlock += UnlockCat;
        InitSuccessList();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DisplaySuccess;
        Entity.OnDeath -= CheckDeathEvent;
        //Mod_Waves.OnBossDefeated -= BossKilled;
        //Mod_Waves.WaveCompleted -= CompleteWaves;
        //Cat.OnUnlock -= UnlockCat;
    }

    private void InitSuccessList()
    {
        allSuccesses.AddRange(Resources.LoadAll<SuccessSO>(successFolderPath));

        if (allSuccesses == null) Debug.Log("Load SucessList Echec");
        Debug.Log("Load SucessList Okay");
    }
    private void DisplaySuccess(Scene arg0, LoadSceneMode arg1)
    {
        if (arg0.name == "Success")
        {
            DisplaySuccessEvent?.Invoke(allSuccesses);
        }
    }

    private void CheckDeathEvent(Entity entity, bool arg2)
    {
        if (entity is Mouse) MouseKilled();
    }
    private void CheckSuccess()
    {
        _successCount++;
        if (_successCount == 5 || _successCount == 10 || _successCount == 20)
        {
            Debug.Log($"Vous avez gagné {_successCount} succès");
            _success["Succès"]++;
        }
        SuccessEvent?.Invoke();

    }

    private void MouseKilled()
    {
        _mouseKilled++;
        if (_mouseKilled == 50 || _mouseKilled == 100 || _mouseKilled == 200 || _mouseKilled == 500 || _mouseKilled == 1000)
        {
            Debug.Log($"Vous avez battu {_mouseKilled} souris");
            _success["Souris battu"]++;
            CheckSuccess();
        } 
    }


   
}