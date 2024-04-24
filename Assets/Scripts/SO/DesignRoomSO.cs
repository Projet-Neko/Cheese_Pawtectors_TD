using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DesignRoomSO", menuName = "Projet Neko/DesignRoomSO")]
public class DesignRoomSO : ScriptableObject
{
    public Mesh mesh;
    public Material[] materials;
}
