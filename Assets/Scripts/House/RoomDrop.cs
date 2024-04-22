using System;
using UnityEngine;

public class RoomDrop : DragAndDropHandler
{
    [SerializeField] private Cat _currentCat;
    [SerializeField] private Room _room;

    public static event Action CatDroped; //Event for the Cat in house Success

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        //Debug.Log(_currentCat == null ? "Room is empty" : "Room is full");
        if (_room.Pattern == RoomPattern.VoidRoom) return;

        bool changeRoom = false;

        if (_currentCat != null)
        {
            if (cat == _currentCat) base.HandleDragAndDrop(cat, initialPosition);
            else if (_currentCat.Level == cat.Level) _currentCat.GetComponent<DragAndDropHandler>().HandleDragAndDrop(cat, initialPosition);
            else base.HandleDragAndDrop(cat, initialPosition);
            return;
        }

        if (cat.IsInStorageMode)
        {
            int previousSlotIndex = int.Parse(cat.transform.parent.name.Split('_')[1]);
            InvokeOnSlotChanged(previousSlotIndex, -1);
            cat.SetStorageMode(false);
        }
        else
        {
            changeRoom = true;
            cat.transform.parent.gameObject.GetComponent<RoomDrop>().ResetRoomSlot();
            InvokeOnRoomChanged((int)cat.transform.parent.transform.position.x, (int)cat.transform.parent.transform.position.y, -1);
        }

        InvokeOnRoomChanged((int)transform.position.x, (int)transform.position.y, cat.Level);

        _currentCat = cat;
        _currentCat.transform.SetParent(transform);
        _currentCat.transform.position = transform.position;
        if (changeRoom) _currentCat.GetComponent<CatBrain>().SetRoom();
        //cat.transform.position = Camera.main.ScreenToWorldPoint(cat.transform.position);
    }

    public override void HandleDragAndDrop(Room room, Vector3 initialPosition)
    {
        Vector3 currentPos = _room.RoundPosition(transform.position);
        Debug.Log($"Dropped {room.Pattern} at {currentPos} [on {_room.Pattern}]");

        if (!room.IsInStorageMode || !House.Instance.AddRoomInGrid(room.Pattern, (int)currentPos.x, (int)currentPos.z))
        {
            base.HandleDragAndDrop(room, initialPosition);
            return;
        }

        room.SetStorageMode(false); // Disable drag and drop
        Destroy(room.transform.parent.parent.gameObject); // Remove storage slot
    }

    public void ResetRoomSlot()
    {
        _currentCat = null;
    }
}