using UnityEngine;

public abstract class DragAndDropHandler : MonoBehaviour
{
    public virtual void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        cat.transform.position = initialPosition;
    }

    public virtual void HandleDragAndDrop(Room room, Vector3 initialPosition)
    {
        room.transform.position = initialPosition;
    }
}