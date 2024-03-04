using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class House : MonoBehaviour
{
    [SerializeField] private List<GameObject> _rooms;

    private const int _maxRooms = 30;
    private const int _minRooms = 5;

    //private List<Room> _roomsAvailable = new List<Room>();

    private GameObject[,] _roomsGrid = new GameObject[_maxRooms,_maxRooms];
    private bool[,] _enableRoomsGrid = new bool[_maxRooms, _maxRooms];

    private int _currentRoomNumber;


    // Start is called before the first frame update
    void Start()
    {
        // Initialize the grid
        for (int i = 0; i < _maxRooms; i++)
        {
            for (int j = 0; j < _maxRooms; j++)
            {
                _enableRoomsGrid[i, j] = false;
            }
        }

        // Create the void rooms visible in the beginning
        _currentRoomNumber = _minRooms;
        for (int i = 0; i < _currentRoomNumber; i++)
        {
            for (int j = 0; j < _currentRoomNumber; j++)
            {
                GameObject room;
                if (i == 0 && j == _currentRoomNumber / 2) // Place the Start Room
                    room = Instantiate(_rooms[3], new Vector3(i, j, 0), Quaternion.identity);
                else// Place Void Rooms
                {
                    room = Instantiate(_rooms[5], new Vector3(i, j, 0), Quaternion.identity);
                    _enableRoomsGrid[i, j] = true;
                }
                room.transform.parent = transform;
                _roomsGrid[i, j] = room;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
