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

    [SerializeField] private GameObject _canva;
    private bool _canvaEnabled = false;
    private bool _canMove;
    private Vector3 _mousePosition;

    protected RoomSecurity _security;
    protected bool[] _openings = new bool[4];

    public RoomSecurity Security { get => _security; }

    // Start is called before the first frame update
    void Start()
    {
        _canMove = false;
    }

    // Update is called once per frame
    private void FixedUpdate()
    {

        if (_canMove)
        {
            _mousePosition =  Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = 0;
            transform.position = _mousePosition;


        }
    }

    public void OnMouseDown()
    {
        if (_canMove)
        {
            _canMove = false;
        }
        else
        {

            _canvaEnabled = !_canvaEnabled;
            _canva.SetActive(_canvaEnabled);
        }
    }

    public void ShowUI()
    {
        _canva.SetActive(true);
    }

    public void RotationClockwise()
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

    public void RotationAnticlockwise()
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

    public void Move()
    {
        _canMove = true;
        _canva.SetActive(false);
    }

    public void Delete()
    {

        Destroy(this.transform.parent.gameObject);
    }


}
