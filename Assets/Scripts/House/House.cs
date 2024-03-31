using AYellowpaper.SerializedCollections;
using UnityEngine;
using System;

public class House : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<RoomPattern, GameObject> _rooms;

    public static event Action<bool> ValidatePositionChange; // bool == position valid� ?

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    private int _currentRoomNumber;

    private Room[,] _roomsGrid = new Room[_maxRooms, _maxRooms];
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
                _roomsGrid[i, j] = room.GetComponentInChildren<Room>();
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
        AddRoom(2, 0, RoomPattern.CrossraodRoom);
        AddRoom(0, 0, RoomPattern.CrossraodRoom);
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
            _roomsGrid[i, _currentRoomNumber - 1] = room.GetComponentInChildren<Room>();
        }

        for (int i = 0; i < _currentRoomNumber - 1; i++)
        {
            GameObject room = Instantiate(_rooms[RoomPattern.VoidRoom], new Vector3(_currentRoomNumber - 1, i, 1), Quaternion.identity);

            room.transform.parent = transform;
            _roomsGrid[_currentRoomNumber - 1, i] = room.GetComponentInChildren<Room>();
        }
    }

    public void AddRoom(int x, int y, RoomPattern pattern)
    {
        Room room = _roomsGrid[x, y];
        if (!room)
        {
            Debug.Log("No room found at position (" + x + ", " + y + ")");
            return;
        }

        if (room.Security == RoomSecurity.Overwritten)
        {
            // Destroy the old room
            _roomsGrid[x, y].Delete();

            // Create the new room
            GameObject roomObject = Instantiate(_rooms[pattern], new Vector3(x, y, 0), Quaternion.identity);
            roomObject.transform.parent = transform;
            _roomsGrid[x, y] = roomObject.GetComponentInChildren<Room>();
        }
        else Debug.Log("Room not overwritable, security = " + _roomsGrid[x, y].Security);
    }

    public void RemoveRoom(int x, int y)
    {
        Room room = _roomsGrid[x, y];
        if (!room)
        {
            Debug.Log("No room found at position (" + x + ", " + y + ")");
            return;
        }

        if (room.Security == RoomSecurity.MovedAndRemoved)
        {
            // Destroy the old room
            _roomsGrid[x, y].Delete();

            // Create the new room
            GameObject roomObject = Instantiate(_rooms[RoomPattern.VoidRoom], new Vector3(x, y, 0), Quaternion.identity);
            roomObject.transform.parent = transform;
            _roomsGrid[x, y] = roomObject.GetComponentInChildren<Room>();
        }
    }

    public void MoveRoom(int xStart, int yStart, int xEnd, int yEnd)
    {
        Room startRoom = _roomsGrid[xStart, yStart];
        Room endRoom = _roomsGrid[xEnd, yEnd];

        if (!endRoom)
        {
            Debug.Log("No room found at position (" + xEnd + ", " + yEnd + ")");
            return;
        }
        if (endRoom.Security == RoomSecurity.Protected) return; // Ajouter une popup "You can't move this room" ?

        // Swap the rooms in the scene
        endRoom.MoveRoom(xStart, yStart);
        startRoom.MoveRoom(xEnd, yEnd);

        // Swap the rooms in the grid
        _roomsGrid[xStart, yStart] = endRoom;
        _roomsGrid[xEnd, yEnd] = startRoom;
    }

    public void DestroyInvalidRoom()
    {
        if (_startRoom.CheckPath())
        {
            for (int i = 0; i < _currentRoomNumber; i++)
            {
                for (int j = 0; j < _currentRoomNumber; j++)
                {
                    Room room = _roomsGrid[i, j];
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
        int xStart = (int)oldPosition.x;
        int yStart = (int)oldPosition.y;
        int xEnd = (int)newPosition.x;
        int yEnd = (int)newPosition.y;

        if (xEnd < 0 || xEnd >= _currentRoomNumber || yEnd < 0 || yEnd >= _currentRoomNumber)
        {
            _roomsGrid[xStart, yStart].MoveRoomOldPosition();
            return;
        }

        if (_roomsGrid[xEnd, yEnd].Security == RoomSecurity.Protected)
        {
            _roomsGrid[xStart, yStart].MoveRoomOldPosition();
            return;
        }

        if (validate)
            MoveRoom(xStart, yStart, xEnd, yEnd);
    }

    private void OnDestroy()
    {
        Room.ChangeTilePosition -= CheckRoomPosition;
    }

}