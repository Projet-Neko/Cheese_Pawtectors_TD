using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Success_Manager : MonoBehaviour
{
    private Dictionary<string, int> _success = new Dictionary<string, int>();

    static public event Action SuccessEvent;

    private int _successCount = 0;
    private int _mouseKilled = 0;
    private int _bossKilled = 0;
    private int _wavesDone = 0;
    private int _catAdopted = 0;
    private int _pieceCount = 0;

    private void Awake()
    {
        Entity.OnDeath += CheckDeathEvent;
        Mod_Waves.OnBossDefeated += BossKilled;
        Mod_Waves.WaveCompleted += CompleteWaves;
        Cat.OnUnlock += UnlockCat;
    }


    void Start()
    {

    }

    private void InitDictionary()
    {
        _success.Add("Succ�s", 0);
        _success.Add("Souris battu", 0);
        _success.Add("Boss battu", 0);
        _success.Add("Vagues termin�s", 0);
        _success.Add("Rejoindre un clan", 0);
        _success.Add("Chat d�bloqu�", 1);
        _success.Add("Changer de photo de profil", 0);
        _success.Add("Changer de pseudo", 0);
        _success.Add("Chat adopt�", 0);
        _success.Add("Vaincre *nom du premier boss*", 0);
        _success.Add("Vaincre *nom du deuxieme boss*", 0);
        _success.Add("Vaincre *nom du troisieme boss*", 0);
        _success.Add("Vaincre *nom du quatri�me boss*", 0);
        _success.Add("Modifier sa maison", 0);
        _success.Add("Nombre de pi�ces dans la maison", 0);
        _success.Add("Nombre de chats dans la maison", 0);
        _success.Add("Donner des chats", 0);
        _success.Add("Activer les notifications", 0);
        _success.Add("Lier son compte", 0);
        _success.Add("Nous suivre sur les r�seaux", 0);
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
            Debug.Log($"Vous avez gagn� {_successCount} succ�s");
            _success["Succ�s"]++;
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

    private void BossKilled()
    {
        _bossKilled++;
        if (_bossKilled == 1 || _bossKilled == 2 || _bossKilled == 5 || _bossKilled == 10 || _bossKilled == 50)
        {
            Debug.Log($"Vous avez battu {_bossKilled} boss");
            _success["Boss battu"]++;
            CheckSuccess();
        }
    }

    private void CompleteWaves()
    {
        _wavesDone++;
        if (_wavesDone == 10 || _wavesDone == 50 || _wavesDone == 100 || _wavesDone == 200 || _wavesDone == 500)
        {
            Debug.Log($"Vous avez termin� {_wavesDone} vagues");
            _success["Vagues termin�es"]++;
            SuccessEvent?.Invoke();
            CheckSuccess();
        }
    }

    private void JoinClan()
    {
        Debug.Log("Vous avez rejoint un clan");
        _success["Rejoindre un clan"]++;
        CheckSuccess();
    }

    private void UnlockCat(int lvl)
    {
        Debug.Log($"Vous avez d�bloqu� le chat de niveau {lvl+1}");
        _success["Chat d�bloqu�"]++;
        CheckSuccess();
    }

    private void ChangeProfilePicture()
    {
        Debug.Log("Vous avez chang� votre photo de profil");
        _success["Changer de photo de profil"]++;
        CheckSuccess();
    }

    private void ChangeUsername()
    {
        Debug.Log("Vous avez chang� votre pseudo");
        _success["Changer de pseudo"]++;
        CheckSuccess();
    }

    private void AdoptCat()
    {
        if (_catAdopted == 10 || _catAdopted == 50 || _catAdopted == 100 || _catAdopted == 200 || _catAdopted == 1000)
        {
            _success["Chat adopt�"]++;
            Debug.Log($"Vous avez adopt� {_catAdopted} chats");
            CheckSuccess();
        }
    }

    private void DefeatBoss1()
    {
        Debug.Log("Vous avez vaincu le premier boss");
        _success["Vaincre *nom du premier boss*"]++;
        CheckSuccess();
    }

    private void DefeatBoss2()
    {
        Debug.Log("Vous avez vaincu le deuxi�me boss");
        _success["Vaincre *nom du deuxi�me boss*"]++;
        CheckSuccess();
    }

    private void DefeatBoss3()
    {
        Debug.Log("Vous avez vaincu le troisi�me boss");
        _success["Vaincre *nom du troisi�me boss*"]++;
        CheckSuccess();
    }

    private void DefeatBoss4()
    {
        Debug.Log("Vous avez vaincu le quatri�me boss");
        _success["Vaincre *nom du quatri�me boss*"]++;
        CheckSuccess();
    }

    private void ModifyHouse()
    {
        Debug.Log("Vous avez modifi� votre maison");
        _success["Modifier sa maison"]++;
        CheckSuccess();
    }

    private void PiecesInHouse()
    {
        if (_pieceCount == 10 || _pieceCount == 15 || _pieceCount == 20 || _pieceCount == 25 || _pieceCount == 30)
        {
            _success["Nombre de pi�ces dans la maison"]++;
            Debug.Log($"Vous avez {_pieceCount} pi�ces dans votre maison");
            CheckSuccess();
        }
    }

    private void CatsInHouse(int catCount)
    {

        if (catCount == 10 || catCount == 20 || catCount == 40 || catCount == 50 || catCount == 100)
        {
            _success["Nombre de chats dans la maison"]++;
            Debug.Log($"Vous avez {catCount} chats dans votre maison");
            CheckSuccess();
        }
    }

    private void GiveCats(int catCount)
    {
        Debug.Log($"Vous avez donn� {catCount} chats");
        _success["Donner des chats"] += catCount;
        CheckSuccess();
    }

    private void EnableNotifications()
    {
        Debug.Log("Vous avez activ� les notifications push");
        _success["Activer les notifications"]++;
        CheckSuccess();
    }

    private void LinkAccount()
    {
        Debug.Log("Vous avez li� votre compte");
        _success["Lier son compte"]++;
        CheckSuccess();
    }

}
