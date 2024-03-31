using System;
using System.Collections.Generic;
using UnityEngine;

public enum RoomPattern
{
    CheeseRoom,     // Moved
    CorridorRoom,   // MovedAndRemoved
    CrossraodRoom,  // MovedAndRemoved
    StartRoom,      // Protected
    TurnRoom,       // MovedAndRemoved
    VoidRoom        // Overwritten
}

public enum RoomSecurity
{
    Protected,          // The room can't be moved or removed
    Moved,              // The room can be moved
    MovedAndRemoved,    // The room can be moved and removed
    Overwritten         // A room can be placed over this one
}

public class Room : MonoBehaviour
{
    [Header("Room")]
    [SerializeField] private GameObject _room;

    [Header("Room Canva")]
    [SerializeField] private GameObject _HUDCanva;
    [SerializeField] private GameObject _moveModCanva;

    [Header("Junction")]
    [SerializeField] protected List<Junction> _opening;

    // Events
    public static event Action<Vector3, Vector3, bool> ChangeTilePosition; // Old position, new position, Still in motion or not (false if the room is still moving)
    public static event Action TileSelected;

    // Getters
    public bool CorrectPath => _correctPath;
    public RoomSecurity Security => _security;

    protected RoomSecurity _security;
    protected bool _correctPath = false;      // True if the room is in a correct path

    private Vector3 _oldPosition;
    private bool _canMove;
    private bool _moveModBool;
    private bool _WaitForValidation = false;
    private bool _isSelected;
    private int _currentLevel = 1;

    // Constants
    private const int _maxLevel = 3;


    //change OnMouseDown to Button to avoid click error
    //Link delete button to delete function of house

    private void OnDestroy()
    {
        // Unsubscribe from events
        TileSelected -= DeselectTile;

        foreach (Junction junction in _opening)
            junction.OnCheckPath -= CheckPath;
    }

    void Start()
    {
        // Subscribe to events
        TileSelected += DeselectTile;
        House.ValidatePositionChange += ChangePosition;

        foreach (Junction junction in _opening)
            junction.OnCheckPath += CheckPath;


        _canMove = false;
        _moveModBool = false;
    }

    private void FixedUpdate()
    {
        if (_moveModBool && _canMove)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _room.transform.position = RoundPosition(mousePosition);
        }
    }

    private Vector3 RoundPosition(Vector3 startPosition)
    {
        Vector3 position = startPosition;
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);
        position.z = 0;

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
        _canMove = true;
        Selected();

        if (!_moveModBool) 
            _HUDCanva.SetActive(!_HUDCanva.activeSelf);
    }

    public void OnMouseUp()
    {
        _canMove = false;
    }

    public void ShowUI()
    {
        _HUDCanva.SetActive(true);
    }

    // When user click on Canvas/HUD/Move button
    public void Move()
    {
        _moveModBool = true;
        _HUDCanva.SetActive(false);                 // Hide the HUD
        _moveModCanva.SetActive(true);              // Show the Move Canvas
        _oldPosition = _room.transform.position;    // Save the currently position
    }

    // When user click on Canvas/Move Arrow/Done button
    public void StopMove()
    {
        _moveModBool = false;
        _WaitForValidation = true;

        _moveModCanva.SetActive(false);                                                 // Hide the Move Canvas
        ChangeTilePosition?.Invoke(_oldPosition, _room.transform.position, true);       //true because the room ask for position validation
    }

    public void Delete()
    {
        Destroy(_room);
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
            if (_HUDCanva != null)
                _HUDCanva.SetActive(false);

            if (_moveModBool == true)
            {
                _moveModBool = false;
                _moveModCanva.SetActive(false);
                MoveRoomOldPosition();
            }
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
            MoveRoomOldPosition();
            //change Material to normal           
            _WaitForValidation = false;
        }
    }

    public void MoveRoom(int x, int y)
    {
        _room.transform.position = new Vector3(x, y, 0);
    }

    public void MoveRoomOldPosition()
    {
        _room.transform.position = _oldPosition;
    }

    private void LevelUp()
    {
        if (_currentLevel < _maxLevel)
            ++_currentLevel;
    }
}