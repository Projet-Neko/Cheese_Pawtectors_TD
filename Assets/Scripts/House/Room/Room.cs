using UnityEngine;

public enum RoomPattern
{
    CheeseRoom,
    CorridorRoom,
    CrossraodRoom,
    StartRoom,
    TurnRoom,
    VoidRoom
}

/*
 * CheeseRoom : Moved
 * CorridorRoom : MovedAndRemoved
 * CrossraodRoom : MovedAndRemoved
 * StartRoom : Protected
 * TurnRoom : MovedAndRemoved
 * VoidRoom : Overwritten
 */
public enum RoomSecurity
{
    Protected,
    Moved,
    MovedAndRemoved,
    Overwritten
}

public class Room : MonoBehaviour
{
    protected RoomSecurity _security;
    protected bool[] _openings = new bool[4];

    public RoomSecurity Security { get => _security; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    protected void RotationClockwise()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z -= 90;

        if (rotation.z < 0)
            rotation.z += 360;

        transform.eulerAngles = rotation;

        bool temp = _openings[3];
        for (int i = 3; i > 0; --i)
            _openings[i] = _openings[i - 1];
        _openings[0] = temp;
    }

    protected void RotationAnticlockwise()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z += 90;

        if (rotation.z >= 360)
            rotation.z -= 360;

        transform.eulerAngles = rotation;

        bool temp = _openings[0];
        for (int i = 0; i < 3; ++i)
            _openings[i] = _openings[i + 1];
        _openings[3] = temp;
    }
}
