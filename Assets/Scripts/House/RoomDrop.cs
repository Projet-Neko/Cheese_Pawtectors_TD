using System;
using UnityEngine;
using UnityEngine.UIElements;

public class RoomDrop : DragAndDropHandler
{
    [SerializeField] private Cat _currentCat;
    [SerializeField] private Room _room;

    private Plane _plane = new Plane(Vector3.up, 0);

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

        _currentCat.SetYPosition();
        if (changeRoom) _currentCat.GetComponent<CatBrain>().SetRoom();
        //cat.transform.position = Camera.main.ScreenToWorldPoint(cat.transform.position);
    }

    public override void HandleDragAndDrop(Room room, Vector3 initialPosition)
    {
        float distance;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (!_plane.Raycast(ray, out distance))
            return;
        
        Vector3 mousePosition = ray.GetPoint(distance);

        int x = (int)Mathf.Round(mousePosition.x);
        int z = (int)Mathf.Round(mousePosition.z);

        if (!room.IsInStorageMode || !House.Instance.AddRoomInGrid(room.Pattern, x, z))
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