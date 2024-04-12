using System;
using UnityEngine;

public abstract class DragAndDropHandler : MonoBehaviour
{
    public static event Action<int, int> OnSlotChanged; // Slot index, cat level

    public virtual void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        cat.transform.position = initialPosition;
    }

    public virtual void HandleDragAndDrop(Room room, Vector3 initialPosition)
    {
        room.transform.position = initialPosition;
    }

    protected void InvokeOnSlotChanged(int slotIndex, int catLevel) => OnSlotChanged?.Invoke(slotIndex, catLevel);
}