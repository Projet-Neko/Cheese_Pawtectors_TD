using System;
using UnityEngine;

public enum CatFur
{
    Solid, Colourpoint, Harlequin, Tabby, Calico, Tortoiseshell
}

[CreateAssetMenu(fileName = "NewCat", menuName = "Projet Neko/Cat")]
public class CatSO : ScriptableObject
{
    public string Name => name.Split("_")[1];
    public int Level => Int32.Parse(name.Split('_')[0]);
    public CatFur Fur;
    public CatColor Color;
    public Sprite SpriteAbove;
    public Sprite SpriteFront;
    public string Description;
}