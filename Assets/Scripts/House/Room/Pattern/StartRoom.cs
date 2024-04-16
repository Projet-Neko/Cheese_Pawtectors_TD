using UnityEngine.Rendering;

public class StartRoom : Room
{
    void Awake()
    {
        _security = RoomSecurity.Protected;
    }
}