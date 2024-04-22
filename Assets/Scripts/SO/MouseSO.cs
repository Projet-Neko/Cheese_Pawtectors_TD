using UnityEngine;
public enum MouseBossType
{
    None,
    RatBoss,
    MouseBallBoss
}

[CreateAssetMenu(fileName = "NewMouse", menuName = "Projet Neko/Mouse")]

public class MouseSO : ScriptableObject
{
    public string Name => name.Split("_")[1];
    public int Health;
    public float Speed;
    public int SatiationRate;
    public Sprite Sprite;
    public MouseBossType MouseBossType;
}