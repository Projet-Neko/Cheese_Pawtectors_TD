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
    [SerializeField] private List<Junction> _opening;

    public RoomSecurity Security => _security;

    private bool _moveModBool;
    private bool _canMove;
    private Vector3 _mousePosition;

    protected RoomSecurity _security;

    public static event Action TileSelected;
    private bool _isSelected;


    private void Awake()
    {
       TileSelected += DeselectTile;
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
        TileSelected?.Invoke(); // On invoque l'évent
    }

    private void DeselectTile()
    {
        if (!_isSelected)
        {
            _HUDCanva.SetActive(false);
        }
        _isSelected = false;

    }

    private void OnDestroy()
    {
        TileSelected -= DeselectTile;
    }
}