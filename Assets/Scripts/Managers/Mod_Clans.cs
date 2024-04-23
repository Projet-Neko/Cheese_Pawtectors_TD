using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Clan
{
    Meowquest, Furrocious, Purrtopia, Whiskerhood
}

public class Mod_Clans : Module
{
    public ClanSO[] ClansData => _clansData;
    private readonly Dictionary<Clan, PlayFab.GroupsModels.EntityKey> _clans = new();

    private readonly List<string> _clansId = new()
    {
        "3D9460C2B375C2FE", // Meowquest
        "6127B58DE8276CDA", // Furrocious
        "9DECC99EF3720FED", // Purrtopia
        "8E0E9E265319D702" // Whiskerhood
    };

    private ClanSO[] _clansData;
    private int[] _quizAnswers;

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        _quizAnswers = new int[_clansId.Count];
        _clansData = Resources.LoadAll<ClanSO>("SO/Clans");
        _clansData.OrderBy(x => x.name);
        StartCoroutine(InitClanList());
    }

    private IEnumerator InitClanList()
    {
        yield return _gm.StartAsyncRequest("Getting clan list...");

        for (int i = 0; i < 4; i++) GetClan(i);

        yield return new WaitUntil(() => _clans.Count == 4);
        _gm.EndRequest();
        InitComplete();
    }

    private void GetClan(int index)
    {
        PlayFabGroupsAPI.GetGroup(new()
        {
            Group = new() { Id = _clansId[index], Type = "group" }
        }, res =>
        {
            _gm.EndRequest();
            _clans[Enum.Parse<Clan>(res.GroupName)] = res.Group;

        }, _gm.OnRequestError);
    }

    private void AddPlayerToClan(Clan clan)
    {
        PlayFabGroupsAPI.ApplyToGroup(new()
        {
            Group = _clans[clan]
        }, res =>
        {
            PlayFabGroupsAPI.AcceptGroupApplication(new()
            {
                Group = _clans[clan]
            }, res =>
            {
                _gm.EndRequest();
                Debug.Log($"Joined {clan} !");
                //StartCoroutine(GetGuildData(PlayerGuild));
            }, _gm.OnRequestError);
        }, _gm.OnRequestError);
    }

    private void Answer(int answerIndex)
    {
        _quizAnswers[answerIndex]++;
    }

    public Clan GetChoosenClan()
    {
        int clan = _quizAnswers[0];

        for (int i = 1; i < _quizAnswers.Length; i++)
        {
            if (_quizAnswers[i] > clan) clan = _quizAnswers[i];
        }

        _gm.Data.UpdateClan(clan);
        AddPlayerToClan((Clan)clan);

        return (Clan)clan;
    }
}