using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<RoomPattern, GameObject> _rooms;
    [SerializeField] private GameObject _mousePrefab;

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    private int _currentRoomNumber;

    private Room[,] _roomsGrid = new Room[_maxRooms, _maxRooms];
    private IdRoom _idStartRoom;

    private List<Mouse> _mouseList = new();

    private bool _isWave = false;

    /* * * * * * * * * * * * * * * * * * * *
     *          BASIC FUNCTIONS
     * * * * * * * * * * * * * * * * * * * */
    void StartPath()
    {
        AddRoom(1, _currentRoomNumber / 2, RoomPattern.CorridorRoom);
        AddRoom(2, _currentRoomNumber / 2, RoomPattern.CorridorRoom);
    }

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
                else if (i == _currentRoomNumber - 2 && j == _currentRoomNumber / 2)
                    CreateRoom(i, j, RoomPattern.CheeseRoom);

                // Place Void Rooms
                else
                    CreateRoom(i, j, RoomPattern.VoidRoom);
            }
        }

        StartPath();

        // Subscribe to events
        Room.ChangeTilePosition += CheckRoomPosition;
        Room.TileDestroyed += CreateRoom;
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

    private void CreateRoom(int x, int z, RoomPattern roomPattern)
    {
        GameObject roomObject = Instantiate(_rooms[roomPattern], new Vector3(x, 0, z), Quaternion.identity);
        roomObject.transform.parent = transform;
        _roomsGrid[x, z] = roomObject.GetComponentInChildren<Room>();
    }

    /*private void CreateRandomRoom()
    {
        int random = UnityEngine.Random.Range(0, 2);
        RoomPattern roomPattern;
        switch (random)
        {
            case 0:
                roomPattern = RoomPattern.CorridorRoom;
                break;
                
            case 1:
                roomPattern = RoomPattern.TurnRoom;
                break;

            case 2:
                roomPattern= RoomPattern.CrossraodRoom;
                break;

            default:
                roomPattern = RoomPattern.CorridorRoom;
                break;
        }
        GameObject roomObject = Instantiate(_rooms[roomPattern], new Vector3(0, 0, 0), Quaternion.identity);
        roomObject.transform.parent = transform;
        _roomsGrid[0, 0] = roomObject.GetComponentInChildren<Room>();
    }*/

    /* * * * * * * * * * * * * * * * * * * *
     *          HUD INTERACTIONS
     * * * * * * * * * * * * * * * * * * * */

    private bool IsInGrid(int x, int z)
    {
        return x >= 0 && x < _currentRoomNumber && z >= 0 && z < _currentRoomNumber;
    }

    private void CheckRoomPosition(Vector3 oldPosition, Vector3 newPosition, bool validate)
    {
        int xStart = (int)oldPosition.x;
        int zStart = (int)oldPosition.z;
        int xEnd = (int)newPosition.x;
        int zEnd = (int)newPosition.z;

        // Check if the new position is in the grid and if the room is movable
        if (!IsInGrid(xEnd, zEnd) || _roomsGrid[xEnd, zEnd].Security == RoomSecurity.Protected)
        {
            _roomsGrid[xStart, zStart].MoveRoomOldPosition();
            return;
        }

        if (validate)
            MoveRoom(xStart, zStart, xEnd, zEnd);
    }

    private void MoveRoom(int xStart, int zStart, int xEnd, int zEnd)
    {
        Room startRoom = _roomsGrid[xStart, zStart];
        Room endRoom = _roomsGrid[xEnd, zEnd];

        if (endRoom.Security == RoomSecurity.Protected) return; // Ajouter une popup "You can't move this room" ?

        // Swap the rooms in the scene
        endRoom.MoveRoom(xStart, zStart);
        startRoom.MoveRoom(xEnd, zEnd);

        // Swap the rooms in the grid
        _roomsGrid[xStart, zStart] = endRoom;
        _roomsGrid[xEnd, zEnd] = startRoom;
    }

    /* * * * * * * * * * * * * * * * * * * *
     *              NEW ROOM
     * * * * * * * * * * * * * * * * * * * */

    private void AddRoom(int x, int z, RoomPattern pattern)
    {
        if (_roomsGrid[x, z].Security == RoomSecurity.Overwritten)
        {
            // Destroy the old room (void)
            _roomsGrid[x, z].Delete();

            // Create the new room
            CreateRoom(x, z, pattern);
        }
        else if (_roomsGrid[x, z].Security == RoomSecurity.MovedAndRemoved)
        {
            //Ajout de la vieille piece dans l'inventaire

            CreateRoom(x, z, pattern);

        }
        else Debug.Log("Room not overwritable, security = " + _roomsGrid[x, z].Security);
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
            for (int z = 0; z < _currentRoomNumber; z++)
            {
                _roomsGrid[x, z].DefineIdRoom(x, z);
                _roomsGrid[x, z].ResetPath();
            }
        }
    }

    public void ResetArrows()
    {
        for (int x = 0; x < _currentRoomNumber; x++)
        {
            for (int z = 0; z < _currentRoomNumber; z++)
                _roomsGrid[x, z].ResetArrows();
        }
    }

    private bool IsPreviousRoom(IdRoom idRoom, IdRoom idRoomSearch)
    {
        foreach (IdRoom idRoomPrevious in _roomsGrid[idRoom.x, idRoom.z].PreviousRooms)
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
        Room room = _roomsGrid[idRoom.x, idRoom.z];                                                                         // Get the room
        room.PreviousRooms.Add(idRoomPrevious);                                                                             // Add the previous room to the list of previous rooms

        if (room.NextRooms.Count != 0)                                                                                      // If the room is already connected to the path under construction
            return true;

        if (room is CheeseRoom)                                                                                             // If the room is the cheese room
        {
            room.ValidatePath();                                                                                            // Validate the path of the cheese room
            return true;
        }

        foreach (Junction junction in room.Opening)                                                                         // Check all the junctions of the room...
        {
            IdRoom idRoomNext = junction.GetIdRoomConnected();                                                              // ... and get the ID of room connected to the junction

            if (idRoomNext.IsNull())                                                                                        // If the junction is not connected to another junction
                continue;

            if (IsPreviousRoom(idRoom, idRoomNext))                                                                         // If the next room is an ancestor of the current room
                continue;

            room.NextRooms.Add(idRoomNext);                                                                                 // Add the next room to the list of next rooms
            junction.ActivateArrow(true);                                                                                   // Activate the arrow of the junction

            bool validPath = BuildPath(idRoomNext, idRoom);                                                                 // Build the path from the next room and check if it is valid

            if (!validPath)                                                                                                 // If the path is not valid...
            {
                room.NextRooms.RemoveAt(room.NextRooms.Count - 1);                                                          // ... remove the next room from the list of next rooms and ...
                junction.ActivateArrow(false);                                                                              // ... deactivate the arrow of the junction
            }
        }

        if (room.NextRooms.Count == 0)                                                                                      // If the room is not connected to any room
        {
            room.PreviousRooms.RemoveAt(room.PreviousRooms.Count - 1);                                                      // Remove the previous room from the list of previous rooms
            return false;
        }
        else
        {
            room.ValidatePath();                                                                                            // Validate the path of the room
            return true;
        }
    }

    public void BuildPath()
    {
        InitBuildPath();                                                                                                    // Define the ID of each room in its junctions

        Room startRoom = _roomsGrid[_idStartRoom.x, _idStartRoom.z];                                                        // Get the start room
        Junction junctionStart = startRoom.Opening[0];                                                                      // Get the junction of the start room
        IdRoom idRoomNext = junctionStart.GetIdRoomConnected();                                                             // Get the ID of the room connected to the junction of the start room

        if (idRoomNext.IsNull())                                                                                            // If the start room is not connected to another room
        {
            Debug.Log("Start room not connected");
            return;
        }

        if (BuildPath(idRoomNext, _idStartRoom))                                                                            // Build the path from the next room and check if it is valid
        {
            startRoom.NextRooms.Add(idRoomNext);                                                                            // Add the next room to the list of next rooms of the start room
            DestroyInvalidRoom();                                                                                           // Destroy the rooms that are not connected to the path
            junctionStart.ActivateArrow(true);                                                                              // Activate the arrow of the junction of the start room
        }
        else
            Debug.Log("Path not valid");
    }

    private void RemoveRoom(int x, int z)
    {
        if (_roomsGrid[x, z].Security == RoomSecurity.MovedAndRemoved)
        {
            // Destroy the old room
            _roomsGrid[x, z].Delete();

            //Ajout dans l'inventaire

            // Create the new room
            CreateRoom(x, z, RoomPattern.VoidRoom);
        }
        else Debug.Log("Room not MovedAndRemoved, security = " + _roomsGrid[x, z].Security);
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

    /* * * * * * * * * * * * * * * * * * * *
    *        Delete room
    * * * * * * * * * * * * * * * * * * * */

    public void DestroyAllRoom()
    {
        for (int i = 0; i < _currentRoomNumber; i++)
        {
            for (int j = 0; j < _currentRoomNumber; j++)
                RemoveRoom(i, j);
        }
    }


    /* * * * * * * * * * * * * * * * * * * *
    *               MOUSE
    * * * * * * * * * * * * * * * * * * * */

    public void StartWave()
    {
        _isWave = true;

        StartCoroutine(SpawnMouse(5));

        StartCoroutine(Wave());
    }

    private IEnumerator Wave()
    {
        Debug.Log("Wave started");
        while (_isWave)
        {
            if (_mouseList.Count == 0)
            {
                Debug.Log("Wave finished");
                _isWave = false;
                yield break;
            }

            for (int i = 0; i < _mouseList.Count; i++)
            {
                Mouse mouse = _mouseList[i];
                Room currentRoom = _roomsGrid[(int)mouse.transform.position.x, (int)mouse.transform.position.z];

                if (mouse.TargetReached())
                {
                    int numberNextRooms = currentRoom.NextRooms.Count;
                    if (numberNextRooms == 0)
                        continue;

                    int random = Random.Range(0, numberNextRooms);
                    mouse.DefineTarget(currentRoom.NextRooms[random]);
                }
                
                mouse.Move();
            }

            yield return null;
        }
    }

    private IEnumerator SpawnMouse(int nbMouse)
    {
        while (nbMouse > 0)
        {
            Mouse mouse = Instantiate(_mousePrefab, new Vector3(_idStartRoom.x, 0.1f, _idStartRoom.z), Quaternion.identity).GetComponent<Mouse>();
            mouse.DefineTarget(_roomsGrid[_idStartRoom.x, _idStartRoom.z].NextRooms[0]);
            _mouseList.Add(mouse);

            --nbMouse;
            yield return new WaitForSeconds(1f);
        }

        yield return null;
    }
}