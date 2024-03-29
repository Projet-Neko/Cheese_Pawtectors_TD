using AYellowpaper.SerializedCollections;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using static UnityEditor.Recorder.OutputPath;
using System;

public class House : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<RoomPattern, GameObject> _rooms;

    public static event Action<bool> ValidatePositionChange; // bool == position validé ?

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    private int _currentRoomNumber;

    private GameObject[,] _roomsGrid = new GameObject[_maxRooms, _maxRooms];
    private StartRoom _startRoom;

    void Start()
    {
        // Create the Void Rooms and one Start Room, visible in the beginning
        _currentRoomNumber = _minRooms;

        for (int i = 0; i < _currentRoomNumber; i++)
        {
            for (int j = 0; j < _currentRoomNumber; j++)
            {
                GameObject room;
                // Place the Start Room
                if (i == 0 && j == _currentRoomNumber / 2)
                {
                    room = Instantiate(_rooms[RoomPattern.StartRoom], new Vector3(i, j, 0), Quaternion.identity);
                    _startRoom = room.GetComponentInChildren<StartRoom>();
                }


                // Place the Cheese Room
                else if (i == _currentRoomNumber - 1 && j == _currentRoomNumber / 2)
                {
                    room = Instantiate(_rooms[RoomPattern.CheeseRoom], new Vector3(i, j, 0), Quaternion.identity);
                }

                // Place Void Rooms
                else
                    room = Instantiate(_rooms[RoomPattern.VoidRoom], new Vector3(i, j, 1), Quaternion.identity);

                room.transform.parent = transform;
                _roomsGrid[i, j] = room;
            }
        }

        Room.ChangeTilePosition += CheckRoomPosition;

        // TO DO : TO REMOVE (is a test)
        AddRoom(1, 2, RoomPattern.CorridorRoom);
        AddRoom(2, 2, RoomPattern.CrossraodRoom);
        AddRoom(2, 3, RoomPattern.CrossraodRoom);
        AddRoom(3, 2, RoomPattern.CorridorRoom);
        AddRoom(1, 0, RoomPattern.TurnRoom);
        AddRoom(3, 0, RoomPattern.TurnRoom);
        AddRoom(4, 0, RoomPattern.TurnRoom);
        //RemoveRoom(2, 1);
    }

    public void ExtendHouse()
    {
        // Security on the max index possible
        if (_currentRoomNumber == _maxRooms)
            return;

        ++_currentRoomNumber;

        for (int i = 0; i < _currentRoomNumber; i++)
        {
            GameObject room = Instantiate(_rooms[RoomPattern.VoidRoom], new Vector3(i, _currentRoomNumber - 1, 1), Quaternion.identity);

            room.transform.parent = transform;
            _roomsGrid[i, _currentRoomNumber - 1] = room;
        }

        for (int i = 0; i < _currentRoomNumber - 1; i++)
        {
            GameObject room = Instantiate(_rooms[RoomPattern.VoidRoom], new Vector3(_currentRoomNumber - 1, i, 1), Quaternion.identity);

            room.transform.parent = transform;
            _roomsGrid[_currentRoomNumber - 1, i] = room;
        }
    }

    public void AddRoom(int x, int y, RoomPattern pattern)
    {
        Room room = _roomsGrid[x, y].GetComponentInChildren<Room>();
        if (!room)
        {
            Debug.Log("No room found at position (" + x + ", " + y + ")");
            return;
        }

        if (room.Security == RoomSecurity.Overwritten)
        {
            // Destroy the old room
            Destroy(_roomsGrid[x, y]);

            // Create the new room
            GameObject roomObject = Instantiate(_rooms[pattern], new Vector3(x, y, 0), Quaternion.identity);
            roomObject.transform.parent = transform;
            _roomsGrid[x, y] = roomObject;
        }
        else Debug.Log("Room not overwritable, security = " + _roomsGrid[x, y].GetComponent<Room>().Security);
    }

    public void RemoveRoom(int x, int y)
    {
        Room room = _roomsGrid[x, y].GetComponentInChildren<Room>();
        if (!room)
        {
            Debug.Log("No room found at position (" + x + ", " + y + ")");
            return;
        }

        if (room.Security == RoomSecurity.MovedAndRemoved)
        {
            // Destroy the old room
            Destroy(_roomsGrid[x, y]);

            // Create the new room
            GameObject roomObject = Instantiate(_rooms[RoomPattern.VoidRoom], new Vector3(x, y, 0), Quaternion.identity);
            roomObject.transform.parent = transform;
            _roomsGrid[x, y] = roomObject;
        }
    }

    public void MoveRoom(int xStart, int yStart, int xEnd, int yEnd)
    {
        Room room = _roomsGrid[xEnd, yEnd].GetComponentInChildren<Room>();
        if (!room)
        {
            Debug.Log("No room found at position (" + xEnd + ", " + yEnd + ")");
            return;
        }

        Room endRoom = _roomsGrid[xEnd, yEnd].GetComponentInChildren<Room>();
        if (endRoom.Security == RoomSecurity.Protected) return; // Ajouter une popup "You can't move this room" ?

        GameObject startRoom = _roomsGrid[xStart, yStart];
        _roomsGrid[xStart, yStart] = _roomsGrid[xEnd, yEnd];
        _roomsGrid[xEnd, yEnd] = startRoom;
        startRoom.transform.position = new Vector3(xEnd, yEnd, 0);
        endRoom.transform.position = new Vector3(xStart, yStart, 0);
        endRoom.UpdateCanvaPosition();
    }

    public void DestroyInvalidRoom()
    {
        if (_startRoom.CheckPath())
        {
            for (int i = 0; i < _currentRoomNumber; i++)
            {
                for (int j = 0; j < _currentRoomNumber; j++)
                {
                    Room room = _roomsGrid[i, j].GetComponentInChildren<Room>();
                    if (!room.CorrectPath)
                        RemoveRoom(i, j);
                }
            }
        }
        else
            Debug.Log("The path is not valid");
    }

    private void CheckRoomPosition(Vector3 oldPosition, Vector3 newPosition, bool validate)
    {
        int xEnd = (int)newPosition.x;
        int yEnd = (int)newPosition.y;

        if (xEnd < 0 || xEnd >= _currentRoomNumber || yEnd < 0 || yEnd >= _currentRoomNumber)
        {
            ValidatePositionChange?.Invoke(false);
            return;
        }

        int xStart = (int)oldPosition.x;
        int yStart = (int)oldPosition.y;

        if (_roomsGrid[xEnd, yEnd].GetComponentInChildren<Room>().Security == RoomSecurity.Protected)
        {
            ValidatePositionChange?.Invoke(false);
            return;
        }


        ValidatePositionChange?.Invoke(true);

        if (validate) MoveRoom(xStart, yStart, xEnd, yEnd);

    }

    private void OnDestroy()
    {
        Room.ChangeTilePosition -= CheckRoomPosition;
    }

}