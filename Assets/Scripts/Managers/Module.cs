using System;
using UnityEngine;

public class Module : MonoBehaviour
{
    public static event Action<Type> OnInitComplete;

    protected GameManager _gm;

    public virtual void Init(GameManager gm) { _gm = gm; }

    protected void InitComplete() => OnInitComplete?.Invoke(GetType());

    protected virtual void DebugOnly()
    {
        Debug.Log($"<color=cyan>--- {GetType().Name} Debug Function ---</color>");
    }
}