using UnityEngine;

public class Mod : MonoBehaviour
{
    protected GameManager _gm;

    public virtual void Init(GameManager gm) { _gm = gm; }

    protected virtual void DebugOnly()
    {
        Debug.Log($"--- {GetType().Name} Debug Function ---");
    }
}