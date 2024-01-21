using UnityEngine;

[CreateAssetMenu(fileName = "NewMouse", menuName = "Projet Neko/Mouse")]
public class MouseSO : ScriptableObject
{
    public int Health;
    public float Speed;
    public int SatiationRate;
    public Sprite Sprite;
}