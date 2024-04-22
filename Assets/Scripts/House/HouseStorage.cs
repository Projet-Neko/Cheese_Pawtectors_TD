using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HouseStorage : MonoBehaviour
{
    [SerializeField] private HorizontalLayoutGroup _container;
    [SerializeField] private GameObject _slotPrefab;

    private List<GameObject> _rooms;


    private void Awake()
    {
        House.OnRoomStored += CreateSlot;
    }

    private void OnDestroy()
    {
        House.OnRoomStored -= CreateSlot;
    }

    private void Start()
    {
        _rooms = new();

        // Temporary inventory without tags
        foreach (var room in House.Instance.RoomsStorage)
        {
            if (room.Key.Item1 is RoomPattern.VoidRoom) continue;
            CreateSlot(room.Key.Item1, room.Key.Item2);
        }
    }

    public void CreateSlot(RoomPattern pattern, RoomDesign design)
    {
        GameObject slot = Instantiate(_slotPrefab, _container.transform);
        GameObject room = Instantiate(House.Instance.Rooms[pattern], slot.transform);
        room.transform.localPosition += new Vector3(0, -2, -20);
        room.transform.localScale = new(200, 200, 200);
        room.transform.localEulerAngles = new Vector3(-95, 0, 0);
        room.GetComponentInChildren<Room>().SetStorageMode(true);
        _rooms.Add(room);
    }
}