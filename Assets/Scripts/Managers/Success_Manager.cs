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

    //private Dictionary<string, int> _success = new Dictionary<string, int>();

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
        Mod_Waves.OnBossDefeated += BossKilledSuccess;
        Mod_Waves.WaveCompleted += WavesSuccess;
        Cat.OnUnlock += CatLevelSuccess;
        InitSuccessList();
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= DisplaySuccess;
        Entity.OnDeath -= CheckDeathEvent;
        Mod_Waves.OnBossDefeated -= BossKilledSuccess;
        Mod_Waves.WaveCompleted -= WavesSuccess;
        Cat.OnUnlock -= CatLevelSuccess;
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

    private SuccessSO FindSuccess(string name)
    {
        foreach (SuccessSO success in allSuccesses)
        {
            if (success.name == name) return success;

        }
        return null;
    }

    private int NextStep(SuccessSO success)
    {
        for (int i = 0; i < success._steps.Count; i++)
        {
            if (success._steps[i] > success._progression) return i;
        }

        return 0;
    }

    private void CheckDeathEvent(Entity entity, bool arg2)
    {
        if (entity is Mouse) MouseKilledSuccess();
    }
    private void Success()
    {
        SuccessSO success = FindSuccess("Success");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 5 || success._progression == 10 || success._progression == 20)
        {
            Debug.Log($"Vous avez gagné {success._progression} succès");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
        }
        //SuccessEvent?.Invoke();

    }

    private void MouseKilledSuccess()
    {
        SuccessSO success = FindSuccess("Souris Vaincus");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 50 || success._progression == 100 || success._progression == 200 || success._progression == 500 || success._progression == 1000)
        {
            Debug.Log($"Vous avez battu {success._progression} souris");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }

    private void BossKilledSuccess()
    {
        SuccessSO success = FindSuccess("Boss Vaincus");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 1 || success._progression == 2 || success._progression == 5 || success._progression == 10 || success._progression == 50)
        {
            Debug.Log($"Vous avez battu {success._progression} boss");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }
    private void CatAdoptedSuccess()
    {
        SuccessSO success = FindSuccess("Chat adopté");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 10 || success._progression == 50 || success._progression == 100 || success._progression == 200 || success._progression == 1000)
        {
            Debug.Log($"Vous avez adopté {success._progression} chats");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }

    private void CatInHouseSuccess()
    {
        SuccessSO success = FindSuccess("Chat dans la maison");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 10 || success._progression == 20 || success._progression == 40)
        {
            Debug.Log($"Vous avez {success._progression} chats dans votre maison");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }

    private void CatGivenSuccess()
    {
        SuccessSO success = FindSuccess("Chat donné");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 1 || success._progression == 5 || success._progression == 10 || success._progression == 25 || success._progression == 50)
        {
            Debug.Log($"Vous avez donné {success._progression} chats");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }

    private void RoomInHouseSuccess()
    {
        SuccessSO success = FindSuccess("Pièces dans la maison");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 10 || success._progression == 15 || success._progression == 20 || success._progression == 25 || success._progression == 30)
        {
            Debug.Log($"Vous avez {success._progression} pièces dans votre maison");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }

    private void ThreatSuccess()
    {
        SuccessSO success = FindSuccess("Threat gagner");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 100 || success._progression == 1000 || success._progression == 100000 || success._progression == 1000000 || success._progression == 100000000)
        {
            Debug.Log($"Vous avez gagné {success._progression} Threats au total");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }

    private void WavesSuccess()
    {
        SuccessSO success = FindSuccess("Waves terminés");
        if (success._complete == true) return;
        success._progression++;
        if (success._progression == 10 || success._progression == 50 || success._progression == 100 || success._progression == 200 || success._progression == 500)
        {
            Debug.Log($"Vous avez terminés {success._progression} vagues");
            if (NextStep(success) == 0) success._complete = true;
            else success._step = NextStep(success);
            Success();
        }
    }

    private void CatLevelSuccess(int lvl)
    {
        SuccessSO success = FindSuccess("Chat débloqué");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez débloqué le chat de niveau {success._progression}");
        if (NextStep(success) == 0) success._complete = true;
        else success._step = NextStep(success);
        Success();
    }

    private void NotificationSucces()
    {
        SuccessSO success = FindSuccess("Activiter notification");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez gagner le succès : Activer les notifications push");
        success._complete = true;
        Success();
    }

    private void ChangePPSuccess()
    {
        SuccessSO success = FindSuccess("Changer de pp");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez gagner le succès : Changer de photo de profil");
        success._complete = true;
        Success();
    }

    private void ChangePseudoSuccess()
    {
        SuccessSO success = FindSuccess("Changer de pseudo");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez gagner le succès : Changer de pseudo");
        success._complete = true;
        Success();
    }


    private void LinkAccountSuccess()
    {
        SuccessSO success = FindSuccess("Lier son compte");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez gagner le succès : Lier son compte");
        success._complete = true;
        Success();
    }

    private void ModifyHouseSuccess()
    {
        SuccessSO success = FindSuccess("Modifier sa maison");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez gagner le succès : Modifier sa maison");
        success._complete = true;
        Success();
    }

    private void ClanJoinedSuccess()
    {
        SuccessSO success = FindSuccess("Rejoindre un clan");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez gagner le succès : Rejoindre un clan");
        success._complete = true;
        Success();
    }

    private void RoomSetSuccess()
    {
        SuccessSO success = FindSuccess("Set de pièce");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez gagner le succès : Set de pièce");
        success._complete = true;
        Success();
    }

    private void Boss1Success()
    {
        SuccessSO success = FindSuccess("Vaincre Boss1");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez vaincu");
        success._complete = true;
        Success();
    } //Non implémenté

    private void Boss2Success()
    {
        SuccessSO success = FindSuccess("Vaincre Boss2");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez vaincu");
        success._complete = true;
        Success();
    } //Non implémenté

    private void Boss3Success()
    {
        SuccessSO success = FindSuccess("Vaincre Boss3");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez vaincu");
        success._complete = true;
        Success();
    } //Non implémenté

    private void Boss4Success()
    {
        SuccessSO success = FindSuccess("Vaincre Boss4");
        if (success._complete == true) return;
        success._progression++;

        Debug.Log($"Vous avez vaincu");
        success._complete = true;
        Success();
    } //Non implémenté




}



