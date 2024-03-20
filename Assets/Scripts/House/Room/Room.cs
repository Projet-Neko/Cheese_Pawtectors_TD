using UnityEngine;

public enum RoomPattern
{
    CheeseRoom, // Moved
    CorridorRoom, // MovedAndRemoved
    CrossraodRoom, // MovedAndRemoved
    StartRoom, // Protected
    TurnRoom, // MovedAndRemoved
    VoidRoom // Overwritten
}

public enum RoomSecurity
{
    Protected,
    Moved,
    MovedAndRemoved,
    Overwritten
}

public class Room : MonoBehaviour
{
    [SerializeField] private GameObject _HUDCanva;
    [SerializeField] private GameObject _moveModCanva;

    public RoomSecurity Security => _security;

    private bool _moveModBool;
    private bool _canMove;
    private Vector3 _mousePosition;

    protected RoomSecurity _security;
    protected bool[] _openings = new bool[4];

    void Start()
    {
        _canMove = false;
        _moveModBool = false;
    }

    private void FixedUpdate()
    {
        if (_moveModBool && _canMove)
        {
            _mousePosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = -1;
            transform.position = _mousePosition;
            _moveModCanva.transform.position = _mousePosition;
        }
    }

    public void OnMouseDown()
    {
         if (!_moveModBool) _HUDCanva.SetActive(!_HUDCanva.activeSelf);
        _canMove = true;
    }

    public void OnMouseUp()
    {
        _canMove = false;
    }

    public void ShowUI()
    {
        _HUDCanva.SetActive(true);
    }

    public void RotationClockwise()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z -= 90;

        if (rotation.z < 0) rotation.z += 360;

        transform.eulerAngles = rotation;

        bool temp = _openings[3];
        for (int i = 3; i > 0; --i) _openings[i] = _openings[i - 1];
        _openings[0] = temp;
    }

    public void RotationAnticlockwise()
    {
        Vector3 rotation = transform.eulerAngles;
        rotation.z += 90;

        if (rotation.z >= 360) rotation.z -= 360;

        transform.eulerAngles = rotation;

        bool temp = _openings[0];
        for (int i = 0; i < 3; ++i) _openings[i] = _openings[i + 1];
        _openings[3] = temp;
    }

    public void Move()
    {
        _moveModBool= true;
        _HUDCanva.SetActive(false);
        _moveModCanva.SetActive(true);
    }

    public void StopMove()
    {
        _moveModBool = false;
        _moveModCanva.SetActive(false);
        _HUDCanva.transform.position = transform.position;
    }

    public void Delete()
    {
        Destroy(transform.parent.gameObject);
    }
}