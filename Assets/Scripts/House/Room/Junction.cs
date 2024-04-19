using System;
using UnityEngine;

public class Junction : MonoBehaviour
{
    [SerializeField] GameObject _arrow;

    public static event Action TileChanged;// Notify the house that a room has changed

    private IdRoom _idRoom;
    private Junction _junctionConnected;

    public IdRoom IdRoom => _idRoom;

    private void Awake()
    {
        _idRoom = new IdRoom(-1, -1);
        _junctionConnected = null;
    }

    private void OnTriggerEnter(Collider collider)
    {
        _junctionConnected = collider.gameObject.GetComponent<Junction>();
        TileChanged?.Invoke();// Notify the house that a room has changed
    }

    private void OnTriggerExit(Collider collider)
    {
        _junctionConnected = null;

        // Check if another junction is connected
        gameObject.SetActive(false);
        gameObject.SetActive(true);

        TileChanged?.Invoke();// Notify the house that a room has changed
    }

    public void SetIdRoom(int x, int z)
    {
        _idRoom = new IdRoom(x, z);
    }

    public IdRoom GetIdRoomConnected()
    {
        if (_junctionConnected != null)
            return _junctionConnected.IdRoom;
        else
            return new IdRoom(-1, -1);
    }

    public void ActivateArrow(bool activate)
    {
        _arrow.SetActive(activate);
    }
}
