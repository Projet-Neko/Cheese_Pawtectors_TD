using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "NewQuestion", menuName = "Projet Neko/ClanQuestion")]
public class ClanQuestionSO : ScriptableObject
{
    public string Question;
    public SerializedDictionary<Clan, string> Answers;
}