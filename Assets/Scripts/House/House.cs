using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RoomType
{
    StartRoom,
    CheeseRoom,
    Corridor,
    CrossraodRoom,
    TurnRoom
}

public class House : MonoBehaviour
{
    [SerializeField] private Dictionary<RoomType, string> _rooms;

    private List<Room> _roomsAvailable = new List<Room>();
    private Room[,] _roomsGrid = new Room[30,30];


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
