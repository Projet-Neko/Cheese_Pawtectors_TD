using UnityEngine;

public enum CatFur
{
    Short, Long
}

[CreateAssetMenu(fileName = "NewCat", menuName = "Projet Neko/Cat")]
public class CatSO : ScriptableObject
{
    public string Name => name.Split("_")[1];
    public int Level => int.Parse(name.Split('_')[0]);
    public Sprite SpriteAbove;
    public Sprite SpriteFront;
    public string Lore;
    public CatState State;
}