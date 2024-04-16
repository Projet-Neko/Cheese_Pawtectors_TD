using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Success", menuName = "Projet Neko/Success")]
public class SuccessSO : ScriptableObject
{
    public string _successame;
    public int _step;
    public string _progression;
    public List<int> _steps;
    public bool _complete;
    
}
