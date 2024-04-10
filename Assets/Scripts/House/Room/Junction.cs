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

        // Check if another junction is connected
        gameObject.SetActive(false);
        gameObject.SetActive(true);
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
