using PlayFab;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Clan
{
    Meowquest, Furrocious, Purrtopia, Whiskerhood
}

public class Mod_Clans : Mod
{
    public static event Action OnInitComplete;

    private readonly Dictionary<Clan, PlayFab.GroupsModels.EntityKey> _clans = new();

    private readonly List<string> _clansId = new()
    {
        "3D9460C2B375C2FE", // Meowquest
        "6127B58DE8276CDA", // Furrocious
        "9DECC99EF3720FED", // Purrtopia
        "8E0E9E265319D702" // Whiskerhood
    };

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        StartCoroutine(InitClanList());
    }

    private IEnumerator InitClanList()
    {
        yield return _gm.StartAsyncRequest("Getting clan list...");

        for (int i = 0; i < 4; i++) GetClan(i);

        yield return new WaitUntil(() => _clans.Count == 4);
        _gm.EndRequest();
        OnInitComplete?.Invoke();
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
}