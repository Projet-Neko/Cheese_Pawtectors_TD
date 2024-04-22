using UnityEngine;

public class DropRoom : DragAndDropHandler
{
    [SerializeField] private Room _room;
    public override void HandleDragAndDrop(Room room, Vector3 initialPosition)
    {
        Debug.Log("HandleDragAndDrop room");
        //Vector3 currentPos = room.RoundPosition(room.transform.position);
        Vector3 currentPos = _room.RoundPosition(transform.position);
        Debug.Log($"Dropped {room.Pattern} at {currentPos}");

        if (!room.IsInStorageMode || !House.Instance.AddRoomInGrid(room.Pattern, (int)currentPos.x, (int)currentPos.z))
        {
            base.HandleDragAndDrop(room, initialPosition);
            return;
        }

        room.SetStorageMode(false); // Disable drag and drop

        Transform slot = room.transform.parent.parent;
        room.gameObject.transform.SetParent(House.Instance.transform);
        Destroy(slot.gameObject); // Remove storage slot
    }
}