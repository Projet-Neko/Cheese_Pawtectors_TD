using UnityEngine;

public class Mod : MonoBehaviour
{
    protected GameManager _gm;

    public virtual void Init(GameManager gm) { _gm = gm; }

    protected virtual void DebugOnly()
    {
        Debug.Log($"<color=cyan>--- {GetType().Name} Debug Function ---</color>");
    }
}