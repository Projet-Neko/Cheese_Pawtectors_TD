using UnityEngine;

public class StartRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Protected;
    }

    void Start()
    {
        CheckPath();
    }

    public bool CheckPath()
    {
        _correctPath = _opening[0].Validation();
        Debug.Log("StartRoom: " + _correctPath);
        return _correctPath;
    }
}