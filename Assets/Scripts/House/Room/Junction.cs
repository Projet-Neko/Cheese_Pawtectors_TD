using System;
using UnityEngine;

public class Junction : MonoBehaviour
{
    private IdRoom _idRoom;
    private IdRoom _idRoomConnected;

    public IdRoom IdRoom => _idRoom;
    public IdRoom IdRoomConnected => _idRoomConnected;

    //private Junction _junctionConnected = null;  // The Junction that this Junction is connected to



    public event Func<Junction, bool> OnCheckPath;

    private void Start()
    {
        _idRoom = new IdRoom(-1, -1);
        _idRoomConnected = new IdRoom(-1, -1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        _idRoomConnected = collision.gameObject.GetComponent<Junction>().IdRoom;
    }

    private void OnCollisionExit(Collision collision)
    {
        _idRoomConnected.SetNull();
    }

    public void SetIdRoom(int x, int y)
    {
        _idRoom = new IdRoom(x, y);
    }

    // Check if the junction is connected to another junction and if the next room is in a valid path
    public bool Validation()
    {
        return true;
        //return _junctionConnected && _junctionConnected.OnCheckPath != null && _junctionConnected.OnCheckPath.Invoke(_junctionConnected);
    }
}
