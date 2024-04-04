using System;
using UnityEngine;

public class Junction : MonoBehaviour
{

    [SerializeField] GameObject _arrow;

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
    }

    private void OnTriggerExit(Collider collider)
    {
        _junctionConnected = null;
    }

    public void SetIdRoom(int x, int y)
    {
        _idRoom = new IdRoom(x, y);
        
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

    public void ActiveArrow2()
    {
        if (_junctionConnected != null)
            ActivateArrow(true);
    }
}
