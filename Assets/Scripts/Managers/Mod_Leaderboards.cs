using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Collections.Generic;

public enum Leaderboards
{
    AllSeasons, CurrentSeason, Weekly, Monthly
}

// TODO -> get clans leaderboard if player has clan
// TODO -> subscribe UpdateLocalLeaderboards to all actions giving score
// TODO -> get more leaderboard entries on scroll

public class Mod_Leaderboards : Module
{
    public static event Action<List<PlayerLeaderboardEntry>> OnGetLeaderboard;

    private void Awake()
    {
        Mod_Account.OnCloudUpdate += UpdateCloudLeaderboards;
    }

    private void OnDestroy()
    {
        Mod_Account.OnCloudUpdate -= UpdateCloudLeaderboards;
    }

    public override void Init(GameManager gm)
    {
        base.Init(gm);
        InitComplete();
    }

    private void UpdateLocalLeaderboards(int value)
    {
        foreach (var leaderboard in _gm.Data.Leaderboards) leaderboard.Value += value;
    }

    private void UpdateCloudLeaderboards()
    {
        List<StatisticUpdate> stats = new();

        foreach (var leaderboard in _gm.Data.Leaderboards)
        {
            stats.Add(new()
            {
                StatisticName = leaderboard.Name,
                Value = leaderboard.Value
            });
        }

        _gm.StartAsyncRequest("Updating player score...");

        PlayFabClientAPI.UpdatePlayerStatistics(new() {
            Statistics = stats
        }, res =>
        {
            //if (!_isInitialized) CompleteInit();
            _gm.EndRequest("Score updated !");
        }, _gm.OnRequestError);
    }

    public void GetLeaderboard(string leaderboard)
    {
        _gm.StartAsyncRequest($"Getting leaderboard {leaderboard}...");

        PlayFabClientAPI.GetLeaderboardAroundPlayer(new()
        {
            StatisticName = leaderboard,
            MaxResultsCount = 100
        }, res =>
        {
            OnGetLeaderboard?.Invoke(res.Leaderboard);
            _gm.EndRequest($"Leaderboard {leaderboard} retrieved !");
        }, _gm.OnRequestError);
    }

    public void GetLeaderboard(string leaderboard, int index)
    {
        _gm.StartAsyncRequest($"Getting leaderboard {leaderboard}...");

        PlayFabClientAPI.GetLeaderboard(new()
        {
            StatisticName = leaderboard,
            MaxResultsCount = 100,
            StartPosition = Math.Clamp(index - 100, 0, index)
    }, res =>
        {
            OnGetLeaderboard?.Invoke(res.Leaderboard);
            _gm.EndRequest($"Leaderboard {leaderboard} retrieved !");
        }, _gm.OnRequestError);
    }

    //private void CompleteInit()
    //{
    //    _isInitialized = true;
    //    OnInitComplete?.Invoke();
    //}

    //private void GetLeaderboards()
    //{
    //    List<string> leaderboards = new();
    //    foreach (var leaderboard in _leaderboards) leaderboards.Add(leaderboard.Key.ToString());

    //    PlayFabClientAPI.GetPlayerStatistics(new()
    //    {
    //        StatisticNames = leaderboards
    //    }, res =>
    //    {
    //        foreach (var leaderboard in res.Statistics)
    //        {
    //            StatisticUpdate stat = new()
    //            {
    //                StatisticName = leaderboard.StatisticName,
    //                Value = leaderboard.Value
    //            };

    //            _leaderboards[Enum.Parse<Leaderboards>(leaderboard.StatisticName)] = stat;
    //            _leadersboardForUpdate.Add(stat);
    //        }

    //        CompleteInit();
    //    }, _gm.OnRequestError);
    //}
}