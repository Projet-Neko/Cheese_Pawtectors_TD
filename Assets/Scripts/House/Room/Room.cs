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

    public RoomSecurity Security => _security;

    private bool _moveModBool;
    private bool _canMove;
    private Vector3 _mousePosition;

    protected bool _correctPath = false;      // True if the room is in a correct path

    public static event Action TileSelected;
    private bool _isSelected;

    protected RoomSecurity _security;

    private void Awake()
    {
        TileSelected += DeselectTile;

        foreach (Junction junction in _opening)
            junction.OnCheckPath += CheckPath;
    }

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
    }

    private void FixedUpdate()
    {
        if (_moveModBool && _canMove)
        {
            _mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _mousePosition.z = -1;
            transform.position = _mousePosition;
            _moveModCanva.transform.position = _mousePosition;
        }
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
                _correctPath = _correctPath || junction.Validation();   // Update of the variable correctPath : if ONE junction is coorect, the room is in a correct path
            }
        }

        Debug.Log("Room: " + _correctPath);
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
            _HUDCanva.SetActive(false);
        }
        _isSelected = false;

    }
}