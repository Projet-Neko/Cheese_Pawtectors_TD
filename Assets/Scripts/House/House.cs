using AYellowpaper.SerializedCollections;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<RoomPattern, GameObject> _rooms;

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    private int _currentRoomNumber;

    private Room[,] _roomsGrid = new Room[_maxRooms, _maxRooms];
    private IdRoom _idStartRoom;

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
                    _idStartRoom = new IdRoom(i, j);
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

    private void InitBuildPath()
    {
        for (int x = 0; x < _currentRoomNumber; x++)
        {
            for (int y = 0; y < _currentRoomNumber; y++)
            {
                _roomsGrid[x, y].DefineIdRoom(x, y);
                _roomsGrid[x, y].ResetPath();
            }
        }
    }

    private bool IsPreviousRoom(IdRoom idRoom, IdRoom idRoomSearch)
    {
        foreach (IdRoom idRoomPrevious in _roomsGrid[idRoom.x, idRoom.y].PreviousRooms)
        {
            if (idRoomPrevious.Equals(idRoomSearch))
                return true;

            if (IsPreviousRoom(idRoomPrevious, idRoomSearch))
                return true;
        }

        return false;
    }

    private bool BuildPath(IdRoom idRoom, IdRoom idRoomPrevious)
    {
        _roomsGrid[idRoom.x, idRoom.y].PreviousRooms.Add(idRoomPrevious);                                                   // Add the previous room to the list of previous rooms

        if (_roomsGrid[idRoom.x, idRoom.y].NextRooms.Count != 0)                                                            // If the room is already connected to the path under construction
            return true;

        if (_roomsGrid[idRoom.x, idRoom.y] is CheeseRoom)                                                                   // If the room is the cheese room
        {
            _roomsGrid[idRoom.x, idRoom.y].ValidatePath();                                                                  // Validate the path of the cheese room
            return true;
        }

        foreach (Junction junction in _roomsGrid[idRoom.x, idRoom.y].Opening)                                               // Check all the junctions of the room...
        {
            IdRoom idRoomNext = junction.GetIdRoomConnected();                                                                // ... and get the ID of room connected to the junction

            if (idRoomNext.IsNull())                                                                                        // If the junction is not connected to another junction
                continue;

            if (IsPreviousRoom(idRoom, idRoomNext))                                                                         // If the next room is an ancestor of the current room
                continue;

            _roomsGrid[idRoom.x, idRoom.y].NextRooms.Add(idRoomNext);                                                       // Add the next room to the list of next rooms

            bool validPath = BuildPath(idRoomNext, idRoom);                                                                 // Build the path from the next room and check if it is valid

            if (!validPath)                                                                                                 // If the path is not valid...
                _roomsGrid[idRoom.x, idRoom.y].NextRooms.RemoveAt(_roomsGrid[idRoom.x, idRoom.y].NextRooms.Count - 1);      // ... remove the next room from the list of next rooms
        }

        if (_roomsGrid[idRoom.x, idRoom.y].NextRooms.Count == 0)                                                            // If the room is not connected to any room
        {
            _roomsGrid[idRoom.x, idRoom.y].PreviousRooms.RemoveAt(_roomsGrid[idRoom.x, idRoom.y].PreviousRooms.Count - 1);  // Remove the previous room from the list of previous rooms
            return false;
        }
        else
        {
            _roomsGrid[idRoom.x, idRoom.y].ValidatePath();                                                                  // Validate the path of the room
            return true;
        }
    }

    public void BuildPath()
    {
        InitBuildPath();                                                                                                    // Define the ID of each room in its junctions
        
        IdRoom idRoomNext = _roomsGrid[_idStartRoom.x, _idStartRoom.y].Opening[0].GetIdRoomConnected();                     // Get the ID of the room connected to the junction of the start room

        if (idRoomNext.IsNull())                                                                                            // If the start room is not connected to another room
        {
            Debug.Log("Start room not connected");
            return;
        }    

        if (BuildPath(idRoomNext, _idStartRoom))                                                                            // Build the path from the next room and check if it is valid
            DestroyInvalidRoom();
        else
            Debug.Log("Path not valid");
    }

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

    private void DestroyInvalidRoom()
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
}