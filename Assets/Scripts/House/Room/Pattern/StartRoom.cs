using System.Collections;
using UnityEngine;

public class StartRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Protected;
    }

    void Start()
    {
        StartCoroutine(Coroutine());
    }

    private IEnumerator Coroutine()
    {
        yield return new WaitForSeconds(2);
        CheckPath();
        yield return null;
    }

    public bool CheckPath()
    {
        _correctPath = _opening[0].Validation();
        Debug.Log("StartRoom: " + _correctPath);
        return _correctPath;
    }
}