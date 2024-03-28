using System;
using System.Collections.Generic;
//using System.Numerics;
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
    [Header("Room Canva")]
    [SerializeField] private GameObject _HUDCanva;
    [SerializeField] private GameObject _moveModCanva;

    [Header("Junction")]
    [SerializeField] protected List<Junction> _opening;

    public static event Action<Vector3, Vector3, bool> ChangeTilePosition; // Old position, new position, Still in motion or not (false if the room is still moving)
    public static event Action TileSelected;
    public bool CorrectPath { get => _correctPath; }
    public RoomSecurity Security => _security;
    public Vector3 _newPosition;
    public Vector3 _oldPosition;


    protected RoomSecurity _security;
    protected bool _correctPath = false;      // True if the room is in a correct path


    private bool _canMove;
    private Vector3 _mousePosition;
    private bool _isSelected;
    private bool _moveModBool;
    private int _currentLevel = 1;
    private bool _WaitForValidation = false;
    private const int _maxLevel = 3;



    //limit the deplacement to the grid
    //link the room to the change position of the house

    private void OnDestroy()
    {
        TileSelected -= DeselectTile;

        foreach (Junction junction in _opening)
            junction.OnCheckPath -= CheckPath;
    }

    void Start()
    {
        _canMove = false;
        _moveModBool = false;

        TileSelected += DeselectTile;
        House.ValidatePositionChange += ChangePosition;

        foreach (Junction junction in _opening)
            junction.OnCheckPath += CheckPath;
    }

    private void FixedUpdate()
    {
        if (_moveModBool && _canMove)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = -1;
            transform.position = _mousePosition;
            transform.position = RoundPosition(transform.position);
            _moveModCanva.transform.position = RoundPosition(_mousePosition);

            if (transform.position != _oldPosition)
                ChangeTilePosition?.Invoke(_oldPosition, _newPosition, false); //false because the room is still moving

        }
    }

    private Vector3 RoundPosition(Vector3 startPosition)
    {
        Vector3 position = startPosition;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        _newPosition = position;
        return position;
    }

    protected virtual bool CheckPath(Junction startJunction)
    {
        // If the room is already in a correct path, return true
        if (_correctPath)
            return true;

        foreach (Junction junction in _opening)                         // Check all the junctions of the room...
        {
            if (junction != startJunction)                              // ... except the one that called the function
            {
                bool aux = junction.Validation();                       // Check if the room is in a correct path. We use a variable aux to avoid not getting into the job because of the OR operator
                _correctPath = _correctPath || aux;                     // Update of the variable correctPath : if ONE junction is coorect, the room is in a correct path
            }
        }

        return _correctPath;
    }

    public void OnMouseDown()
    {
        Selected();
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

    public void Move()
    {
        _moveModBool = true;
        _HUDCanva.SetActive(false);
        _moveModCanva.SetActive(true);
        _oldPosition = transform.position;
    }

    public void StopMove()
    {
        _moveModBool = false;
        _moveModCanva.SetActive(false);
        _HUDCanva.transform.position = new Vector3(transform.position.x, transform.position.y, -5);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        ChangeTilePosition?.Invoke(_oldPosition, _newPosition, true); //true because the room ask for position validation
        _WaitForValidation = true;
    }

    public void Delete()
    {
        Destroy(transform.parent.gameObject);
    }

    public void RotationRoom(bool clockwise)
    {

        Vector3 rotation = transform.eulerAngles;

        if (clockwise)
        {
            rotation.z -= 90;

            if (rotation.z >= 360) rotation.z -= 360;

        }
        else
        {
            rotation.z += 90;

            if (rotation.z < 0) rotation.z += 360;
        }

        transform.eulerAngles = rotation;
    }

    private void Selected()
    {
        _isSelected = true;
        TileSelected?.Invoke(); // On invoque l'�vent
    }

    private void DeselectTile()
    {

        if (!_isSelected)
        {
            if (_HUDCanva != null) _HUDCanva.SetActive(false);
            if (_moveModCanva != null) StopMove();
        }
        _isSelected = false;

    }

    private void ChangePosition(bool validate)
    {

        if (_moveModBool)
        {
            if (validate)
            {
                //change Material to green
            }
            else
            {
                //change Material to red
            }
        }
        if (_WaitForValidation)
        {
            transform.position = new Vector3(0, 0, 0);
            //change Material to normal           
            _WaitForValidation = false;
        }
    }

    //
    private void LevelUp()
    {
        if (_currentLevel < _maxLevel)
            ++_currentLevel;
    }
}