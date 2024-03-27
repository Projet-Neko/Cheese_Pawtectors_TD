using AYellowpaper.SerializedCollections;
using UnityEngine;

public class House : MonoBehaviour
{
    [SerializeField] private SerializedDictionary<RoomPattern, GameObject> _rooms;

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    private int _currentRoomNumber;

    //private List<Room> _roomsAvailable = new List<Room>();

    private GameObject[,] _roomsGrid = new GameObject[_maxRooms, _maxRooms];

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
                    room.GetComponentInChildren<Room>().SetupRoom();
                }
                    

                // Place the Cheese Room
                else if (i == _currentRoomNumber - 1 && j == _currentRoomNumber / 2)
                {
                    room = Instantiate(_rooms[RoomPattern.CheeseRoom], new Vector3(i, j, 0), Quaternion.identity);
                    room.GetComponentInChildren<Room>().SetupRoom();
                }

                // Place Void Rooms
                else
                    room = Instantiate(_rooms[RoomPattern.VoidRoom], new Vector3(i, j, 1), Quaternion.identity);

                room.transform.parent = transform;
                _roomsGrid[i, j] = room;
            }
        }

        // TO DO : TO REMOVE (is a test)
        AddRoom(1, 2, RoomPattern.CorridorRoom);
        AddRoom(2, 2, RoomPattern.CrossraodRoom);
        AddRoom(2, 3, RoomPattern.CrossraodRoom);
        AddRoom(3, 2, RoomPattern.CorridorRoom);
        //RemoveRoom(2, 1);
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

        _roomsGrid[x, y].GetComponentInChildren<Room>().SetupRoom();
    }

    public void RemoveRoom(int x, int y)
    {
        Room room = _roomsGrid[x, y].GetComponent<Room>();
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
        Room room = _roomsGrid[xEnd, yEnd].GetComponent<Room>();
        if (!room)
        {
            Debug.Log("No room found at position (" + xEnd + ", " + yEnd + ")");
            return;
        }

        RoomSecurity roomSecurityStart = _roomsGrid[xStart, yStart].GetComponent<Room>().Security;
        if ((roomSecurityStart == RoomSecurity.MovedAndRemoved || roomSecurityStart == RoomSecurity.Moved) && _roomsGrid[xStart, yStart].GetComponent<Room>().Security == RoomSecurity.Overwritten)
        {
            GameObject tmpRoom = _roomsGrid[xStart, yStart];
            _roomsGrid[xStart, yStart] = _roomsGrid[xEnd, yEnd];
            _roomsGrid[xEnd, yEnd] = tmpRoom;
        }
    }
}