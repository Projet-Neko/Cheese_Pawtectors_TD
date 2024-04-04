using AYellowpaper.SerializedCollections;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<RoomPattern, GameObject> _rooms;

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    private int _currentRoomNumber;

    private Room[,] _roomsGrid = new Room[_maxRooms, _maxRooms];
    private StartRoom _startRoom;

    /* * * * * * * * * * * * * * * * * * * *
     *          BASIC FUNCTIONS
     * * * * * * * * * * * * * * * * * * * */
    void Start()
    {
        // Create the Void Rooms and one Start Room, visible in the beginning
        _currentRoomNumber = _minRooms;

        for (int i = 0; i < _currentRoomNumber; i++)
        {
            for (int j = 0; j < _currentRoomNumber; j++)
            {
                // Place the Start Room
                if (i == 0 && j == _currentRoomNumber / 2)
                {
                    CreateRoom(i, j, RoomPattern.StartRoom);
                    _startRoom = (StartRoom)_roomsGrid[i, j];
                }

                // Place the Cheese Room
                else if (i == _currentRoomNumber - 1 && j == _currentRoomNumber / 2)
                    CreateRoom(i, j, RoomPattern.CheeseRoom);

                // Place Void Rooms
                else
                    CreateRoom(i, j, RoomPattern.VoidRoom);
            }
        }

        // Subscribe to events
        Room.ChangeTilePosition += CheckRoomPosition;
        Room.TileDestroyed += CreateRoom;

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

    private void OnDestroy()
    {
        // Unsubscribe to events
        Room.ChangeTilePosition -= CheckRoomPosition;
        Room.TileDestroyed -= CreateRoom;
    }


    /* * * * * * * * * * * * * * * * * * * *
     *          CREATE ROOM
     * * * * * * * * * * * * * * * * * * * */

    private void CreateRoom(int x, int y, RoomPattern roomPattern)
    {
        GameObject roomObject = Instantiate(_rooms[roomPattern], new Vector3(x, y, 0), Quaternion.identity);
        roomObject.transform.parent = transform;
        _roomsGrid[x, y] = roomObject.GetComponentInChildren<Room>();
    }


    /* * * * * * * * * * * * * * * * * * * *
     *          HUD INTERACTIONS
     * * * * * * * * * * * * * * * * * * * */

    private bool IsInGrid(int x, int y)
    {
        return x >= 0 && x < _currentRoomNumber && y >= 0 && y < _currentRoomNumber;
    }

    private void CheckRoomPosition(Vector3 oldPosition, Vector3 newPosition, bool validate)
    {
        int xStart = (int)oldPosition.x;
        int yStart = (int)oldPosition.y;
        int xEnd = (int)newPosition.x;
        int yEnd = (int)newPosition.y;

        // Check if the new position is in the grid and if the room is movable
        if (!IsInGrid(xEnd, yEnd) || _roomsGrid[xEnd, yEnd].Security == RoomSecurity.Protected)
        {
            _roomsGrid[xStart, yStart].MoveRoomOldPosition();
            return;
        }

        if (validate)
            MoveRoom(xStart, yStart, xEnd, yEnd);
    }

    private void MoveRoom(int xStart, int yStart, int xEnd, int yEnd)
    {
        Room startRoom = _roomsGrid[xStart, yStart];
        Room endRoom = _roomsGrid[xEnd, yEnd];

        if (endRoom.Security == RoomSecurity.Protected) return; // Ajouter une popup "You can't move this room" ?

        // Swap the rooms in the scene
        endRoom.MoveRoom(xStart, yStart);
        startRoom.MoveRoom(xEnd, yEnd);

        // Swap the rooms in the grid
        _roomsGrid[xStart, yStart] = endRoom;
        _roomsGrid[xEnd, yEnd] = startRoom;
    }

    /* * * * * * * * * * * * * * * * * * * *
     *              NEW ROOM
     * * * * * * * * * * * * * * * * * * * */

    private void AddRoom(int x, int y, RoomPattern pattern)
    {
        if (_roomsGrid[x, y].Security == RoomSecurity.Overwritten)
        {
            // Destroy the old room
            _roomsGrid[x, y].Delete();

            // Create the new room
            CreateRoom(x, y, pattern);
        }
        else Debug.Log("Room not overwritable, security = " + _roomsGrid[x, y].Security);
    }

    public void ExtendHouse()
    {
        // Security on the max index possible
        if (_currentRoomNumber == _maxRooms) return;

        ++_currentRoomNumber;

        // Create the new rooms on the top
        for (int i = 0; i < _currentRoomNumber; i++)
            CreateRoom(i, _currentRoomNumber - 1, RoomPattern.VoidRoom);

        // Create the new rooms on the right
        for (int i = 0; i < _currentRoomNumber - 1; i++)
            CreateRoom(_currentRoomNumber - 1, i, RoomPattern.VoidRoom);
    }

    /* * * * * * * * * * * * * * * * * * * *
    *        VALIDATION OF THE PATH
    * * * * * * * * * * * * * * * * * * * */

    private void RemoveRoom(int x, int y)
    {
        if (_roomsGrid[x, y].Security == RoomSecurity.MovedAndRemoved)
        {
            // Destroy the old room
            _roomsGrid[x, y].Delete();

            // Create the new room
            CreateRoom(x, y, RoomPattern.VoidRoom);
        }
        else Debug.Log("Room not MovedAndRemoved, security = " + _roomsGrid[x, y].Security);
    }

    public void DestroyInvalidRoom()
    {
        if (_startRoom.CheckPath())
        {
            for (int i = 0; i < _currentRoomNumber; i++)
            {
                for (int j = 0; j < _currentRoomNumber; j++)
                {
                    if (!_roomsGrid[i, j].CorrectPath)
                        RemoveRoom(i, j);
                }
            }
        }
        else
            Debug.Log("The path is not valid");
    }

    /* * * * * * * * * * * * * * * * * * * *
    *        Delete room
    * * * * * * * * * * * * * * * * * * * */

    public void DestroyAllRoom()
    {
        for (int i = 0; i< _currentRoomNumber; i++)
        {
            for (int j = 0; j < _currentRoomNumber; j++)
            {
                //Ajouter les rooms a une liste ? 
                // Rajouter toutes les rooms de la liste à l'inventaire ?
                //ou juste les ajouter une par une
                RemoveRoom(i, j);
            }
        }

    }

}