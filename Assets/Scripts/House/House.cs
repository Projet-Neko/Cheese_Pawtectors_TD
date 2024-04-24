using AYellowpaper.SerializedCollections;
using NaughtyAttributes;
using System;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour
{
    public static House Instance { get; private set; }

    public static event Action<RoomPattern> OnRoomStored;

    [Header("Prefabs")]
    [SerializeField] private SerializedDictionary<RoomPattern, GameObject> _rooms;
    [SerializeField] private GameObject _mousePrefab;
    [SerializeField] private GameObject _linePrefab;

    [Header("Dependencies")]
    [SerializeField] private GameObject _lineObject;

    [Header("Scene where player can use HUD")]
    [SerializeField, Scene] private string _sceneHUD;

    public SerializedDictionary<RoomPattern, GameObject> Rooms => _rooms;
    public Dictionary<Tuple<RoomPattern, RoomDesign>, int> RoomsStorage => _roomsStorage;
    public int MaxRooms => _maxRooms;

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    private int _currentRoomNumber;

    private Room[,] _roomsGrid = new Room[_maxRooms, _maxRooms];
    private LineRenderer[,] _lineGrid = new LineRenderer[2, _maxRooms+1];
    private IdRoom _idStartRoom;
    private bool _pathBuilt = false;

    private Dictionary<Tuple<RoomPattern, RoomDesign>, int> _roomsStorage = new();

    /* * * * * * * * * * * * * * * * * * * *
     *          BASIC FUNCTIONS
     * * * * * * * * * * * * * * * * * * * */

    private bool CreateInstance()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return false;
        }
        Instance = this;
        DontDestroyOnLoad(this);
        return true;
    }

    private void AddLine(float x, float z, float xEnd, float zEnd)
    {
        int indice1 = 1;
        int indice2 = (int)z;
        if (x == xEnd)
        {
            indice1 = 0;
            indice2 = (int)x;
        }

        x -= 0.5f;
        z -= 0.5f;
        xEnd -= 0.5f;
        zEnd -= 0.5f;

        GameObject gameObject = Instantiate(_linePrefab, new Vector3(x, 0, z), Quaternion.identity);
        gameObject.transform.parent = _lineObject.transform;

        _lineGrid[indice1, indice2] = gameObject.GetComponent<LineRenderer>();
        _lineGrid[indice1, indice2].SetPosition(0, new Vector3(x, 0, z));
        _lineGrid[indice1, indice2].SetPosition(1, new Vector3(xEnd, 0, zEnd));
    }

    void StartPath()
    {
        AddRoom(1, _maxRooms / 2, RoomPattern.CorridorRoom);
        AddRoom(2, _maxRooms / 2, RoomPattern.CorridorRoom);

        /** TEST WITH PATH MORE COMPLEXE **/
        /*AddRoom(1, _maxRooms / 2, RoomPattern.CrossraodRoom);
        AddRoom(2, _maxRooms / 2, RoomPattern.CrossraodRoom);
        AddRoom(1, _maxRooms / 2 - 1, RoomPattern.CrossraodRoom);
        AddRoom(2, _maxRooms / 2 - 1, RoomPattern.CrossraodRoom);
        AddRoom(1, _maxRooms / 2 + 1, RoomPattern.CrossraodRoom);
        AddRoom(2, _maxRooms / 2 + 1, RoomPattern.CrossraodRoom);*/
    }

    void Start()
    {
        if (!CreateInstance()) return;

        // Create the Void Rooms and one Start Room, visible in the beginning
        _currentRoomNumber = _minRooms;

        int startZ = _maxRooms / 2 - _currentRoomNumber / 2;

        for (int x = 0; x < _currentRoomNumber; x++)
        {
            AddLine(x, startZ, x, startZ + _currentRoomNumber);
            for (int z = startZ; z < startZ + _currentRoomNumber; z++)
            {
                AddLine(0, z, _currentRoomNumber, z);
                // Place the Start Room
                if (x == 0 && z == _maxRooms / 2)
                {
                    CreateRoom(x, z, RoomPattern.StartRoom);
                    _idStartRoom = new IdRoom(x, z);
                }

                // Place the Cheese Room
                else if (x == _currentRoomNumber - 2 && z == _maxRooms / 2)
                    CreateRoom(x, z, RoomPattern.CheeseRoom);

                // Place Void Rooms
                else
                    CreateRoom(x, z, RoomPattern.VoidRoom);
            }
        }

        AddLine(_currentRoomNumber, startZ, _currentRoomNumber, startZ + _currentRoomNumber);
        AddLine(0, startZ + _currentRoomNumber, _currentRoomNumber, startZ + _currentRoomNumber);

        StartPath();

        // Subscribe to events
        Room.ChangeTilePosition += CheckRoomPosition;
        Room.TileDestroyed += RemoveRoom;
        Room.LineActivated += ActiveLine;
        Junction.TileChanged += BuildPath;
        MouseBrain.VisitedNextRoom += GetNextTarget;
        Mod_Waves.OnBossDefeated += ExtendHouse;
}

    private void OnDestroy()
    {
        // Unsubscribe to events
        Room.ChangeTilePosition -= CheckRoomPosition;
        Room.TileDestroyed -= RemoveRoom;
        Room.LineActivated -= ActiveLine;
        Junction.TileChanged -= BuildPath;
        MouseBrain.VisitedNextRoom -= GetNextTarget;
        Mod_Waves.OnBossDefeated -= ExtendHouse;
    }


    /* * * * * * * * * * * * * * * * * * * *
     *          CREATE ROOM
     * * * * * * * * * * * * * * * * * * * */

    private void CreateRoom(int x, int z, RoomPattern roomPattern)
    {
        GameObject roomObject = Instantiate(_rooms[roomPattern], new Vector3(x, 0, z), Quaternion.identity);
        roomObject.transform.parent = transform;
        Room room = roomObject.GetComponentInChildren<Room>();
        room.SceneForHUD(_sceneHUD);
        _roomsGrid[x, z] = room;
    }

    /* * * * * * * * * * * * * * * * * * * *
     *          HUD INTERACTIONS
     * * * * * * * * * * * * * * * * * * * */

    public void ActiveLine(bool enable)
    {
        _lineObject.SetActive(enable);
    }

    private bool IsInGrid(int x, int z)
    {
        int startZ = _maxRooms / 2 - _currentRoomNumber / 2;
        return x >= 0 && x < _currentRoomNumber && z >= startZ && z < startZ + _currentRoomNumber;
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

    private void ReplaceRoom(int x, int z, RoomPattern pattern)
    {
        // Destroy the old room in the grid
        _roomsGrid[x, z].Delete();

        // Create the new room
        CreateRoom(x, z, pattern);

        // Build the new path
        BuildPath();
    }

    private void AddRoom(int x, int z, RoomPattern pattern)
    {
        if (_roomsGrid[x, z].Security == RoomSecurity.Overwritten)
        {
            ReplaceRoom(x, z, pattern);
        }
        else if (_roomsGrid[x, z].Security == RoomSecurity.MovedAndRemoved)
        {
            AddRoomInInventory(_roomsGrid[x, z].Pattern);// Add old room in inventory

            ReplaceRoom(x, z, pattern);
        }
        else Debug.Log("Room not overwritable, security = " + _roomsGrid[x, z].Security);
    }

    private void UpdateLineRight()
    {
        int indice = _maxRooms / 2 - _currentRoomNumber / 2;

        for (int i = 0; i <= _currentRoomNumber; i++)
            _lineGrid[1,i + indice].SetPosition(1, new Vector3(_currentRoomNumber - 0.5f, 0, i - 0.5f + indice));
    }

    private void UpdateLineHight(int direction)
    {
        int point = direction == 1 ? 1 : 0;

        for (int i = 0; i < _currentRoomNumber; i++)
            _lineGrid[0, i].SetPosition(point, new Vector3(i - 0.5f, 0, _maxRooms / 2 + direction * (-_currentRoomNumber / 2 + _currentRoomNumber + 0.5f - point)));
    }

    public void ExtendHouse()
    {
        // Security on the max index possible
        if (_currentRoomNumber == _maxRooms) return;

        ++_currentRoomNumber;

        int zStart = _maxRooms / 2 - _currentRoomNumber / 2;

        // Create the new rooms on the top/bottom
        if (_currentRoomNumber % 2 == 0)// Create the new rooms on the bottom
        {
            for (int x = 0; x < _currentRoomNumber; x++)
                CreateRoom(x, zStart, RoomPattern.VoidRoom);

            UpdateLineHight(-1);
            AddLine(0, zStart, _currentRoomNumber, zStart);
        }
        else// Create the new rooms on the top
        {
            int zEnd = _maxRooms / 2 + _currentRoomNumber / 2;
            for (int x = 0; x < _currentRoomNumber; x++)
                CreateRoom(x, zEnd, RoomPattern.VoidRoom);

            UpdateLineHight(1);
            AddLine(0, zEnd + 1, _currentRoomNumber, zEnd + 1);
        }

        // Create the new rooms on the right
        for (int z = 0; z < _currentRoomNumber - 1; z++)
            CreateRoom(_currentRoomNumber - 1, z, RoomPattern.VoidRoom);

        UpdateLineRight();
        AddLine(_currentRoomNumber, zStart, _currentRoomNumber, zStart + _currentRoomNumber);

        AddRandomRoomInInventory(); // Give new room to the player
    }

    /* * * * * * * * * * * * * * * * * * * *
    *        VALIDATION OF THE PATH
    * * * * * * * * * * * * * * * * * * * */

    private void InitBuildPath()
    {
        int zStart = _maxRooms / 2 - _currentRoomNumber / 2;

        for (int x = 0; x < _currentRoomNumber; x++)
        {
            for (int z = zStart; z < zStart + _currentRoomNumber; z++)
            {
                _roomsGrid[x, z].DefineIdRoom(x, z);
                _roomsGrid[x, z].ResetPath();
            }
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

            bool validPath = BuildPath(idRoomNext, idRoom);                                                                 // Build the path from the next room and check if it is valid

            if (!validPath)                                                                                                 // If the path is not valid...
                room.NextRooms.RemoveAt(room.NextRooms.Count - 1);                                                          // ... remove the next room from the list of next rooms
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

    private void BuildPath()
    {
        Debug.Log("Build path");
        InitBuildPath();                                                                                                    // Define the ID of each room in its junctions

        Room startRoom = _roomsGrid[_idStartRoom.x, _idStartRoom.z];                                                        // Get the start room
        startRoom.ValidatePath();                                                                                           // Validate the path of the start room
        Junction junctionStart = startRoom.Opening[0];                                                                      // Get the junction of the start room
        IdRoom idRoomNext = junctionStart.GetIdRoomConnected();                                                             // Get the ID of the room connected to the junction of the start room

        if (idRoomNext.IsNull())                                                                                            // If the start room is not connected to another room
            _pathBuilt = false;                                                                                             // Initialize the path as not valid
        else
            _pathBuilt = BuildPath(idRoomNext, _idStartRoom);                                                               // Build the path from the next room and check if it is valid
        
        ColorInvalidRoom();                                                                                                 // Color the rooms that are not connected to the path in red
    }

    public bool ValidatePath()
    {
        if (_pathBuilt)
        {
            Room startRoom = _roomsGrid[_idStartRoom.x, _idStartRoom.z];                                                    // Get the start room
            IdRoom idRoomNext = startRoom.Opening[0].GetIdRoomConnected();                                                  // Get the ID of the room connected to the junction of the start room
            startRoom.NextRooms.Add(idRoomNext);                                                                            // Add the next room to the list of next rooms of the start room
            return true;
        }
        else
        {
            Debug.Log("Path not valid");
            return false;
        }
    }

    private void RemoveRoom(int x, int z)
    {
        if (_roomsGrid[x, z].Security == RoomSecurity.MovedAndRemoved)
        {
            AddRoomInInventory(_roomsGrid[x, z].Pattern);// Add old room to the inventory

            ReplaceRoom(x, z, RoomPattern.VoidRoom);
        }
    }

    private void ColorInvalidRoom()
    {
        int zStart = _maxRooms / 2 - _currentRoomNumber / 2;

        for (int i = 0; i < _currentRoomNumber; i++)
        {
            for (int j = zStart; j < zStart + _currentRoomNumber; j++)
            {
                if (_roomsGrid[i, j].Security == RoomSecurity.Overwritten)
                    continue;

                if (!_roomsGrid[i, j].CorrectPath)
                    _roomsGrid[i, j].ColorInvalidRoom(true);
                else
                    _roomsGrid[i, j].ColorInvalidRoom(false);
            }
        }
    }

    public void DestroyInvalidRoom()
    {
        int zStart = _maxRooms / 2 - _currentRoomNumber / 2;

        for (int i = 0; i < _currentRoomNumber; i++)
        {
            for (int j = zStart; j < zStart + _currentRoomNumber; j++)
            {
                if (!_roomsGrid[i, j].CorrectPath)
                    RemoveRoom(i, j);
            }
        }
    }

    /* * * * * * * * * * * * * * * * * * * *
    *        Delete room
    * * * * * * * * * * * * * * * * * * * */

    // Button to delete all the rooms
    public void DestroyAllRoom()
    {
        int zStart = _maxRooms / 2 - _currentRoomNumber / 2;

        for (int i = 0; i < _currentRoomNumber; i++)
        {
            for (int j = zStart; j < zStart + _currentRoomNumber; j++)
                RemoveRoom(i, j);
        }
    }


    /* * * * * * * * * * * * * * * * * * * *
    *               MOUSE
    * * * * * * * * * * * * * * * * * * * */

    private GameObject GetNextTarget (Vector3 position)
    {
        Room currentRoom = _roomsGrid[(int)position.x, (int)position.z];
        int numberNextRooms = currentRoom.NextRooms.Count;

        if (numberNextRooms == 0)
            return GameManager.Instance.Cheese.gameObject;

        int random = UnityEngine.Random.Range(0, numberNextRooms);
        IdRoom idRoom = currentRoom.NextRooms[random];
        return _roomsGrid[idRoom.x, idRoom.z].gameObject;
    }


    /* * * * * * * * * * * * * * * * * * * *
    *           INVENTORY ROOM
    * * * * * * * * * * * * * * * * * * * */

    private void AddRandomRoomInInventory()
    {
        RoomPattern roomPattern;
        int random = UnityEngine.Random.Range(0, 100);

        switch(random)
        {
            case int n when n < 10:
                roomPattern = RoomPattern.CrossraodRoom;
                break;
            case int n when n < 30:
                roomPattern = RoomPattern.IntersectionRoom;
                break;
            case int n when n < 60:
                roomPattern = RoomPattern.TurnRoom;
                break;
            default:
                roomPattern = RoomPattern.CorridorRoom;
                break;
        }

        AddRoomInInventory(roomPattern);

        /*GameObject roomObject = Instantiate(_rooms[roomPattern], new Vector3(0, 0, 0), Quaternion.identity);
        roomObject.transform.parent = transform;
        _roomsGrid[0, 0] = roomObject.GetComponentInChildren<Room>();*/
    }

    private void AddRoomInInventory(RoomPattern roomPattern)
    {
        Debug.Log("Add room in inventory");
        OnRoomStored.Invoke(roomPattern);
    }

    public bool AddRoomInGrid(RoomPattern roomPattern, int x, int z)
    {
        RoomPattern oldRoomPattern = _roomsGrid[x, z].Pattern;
        if (IsInGrid(x, z) && oldRoomPattern != RoomPattern.StartRoom && oldRoomPattern != RoomPattern.CheeseRoom)
        {
            AddRoom(x, z, roomPattern);
            return true;
        }

        return false;
    }


    /* * * * * * * * * * * * * * * * * * * *
    *           ROOM POSITION AUTO
    * * * * * * * * * * * * * * * * * * * */

    private bool RoomPositionAuto(IdRoom previousRoom, IdRoom currentRoom)
    {
        if (!IsInGrid(currentRoom.x, currentRoom.z))
            return false;

        // Si on n'a plus de room � placer --> return true

        // Choisir une room al�atoire
        // tourner la room jusqu'� ce que la jointure soit dans la bonne direction
        // selectionner une autre jointure
        // rappeler la fonction avec la nouvelle room
        // si la fonction retourne false, on recommence avec une autre jointure, sinon on retourne true
        // si on a test� toutes les jointures et qu'aucune ne fonctionne, on retourne false

        return true;
    }

    public void RoomPositionAuto()
    {
        DestroyAllRoom();

        IdRoom idRoom = new IdRoom(_idStartRoom.x+1, _idStartRoom.z);

        // Choisir une room al�atoire
        // Orienter une jointure de la room vers la room pr�c�dente
    }
}