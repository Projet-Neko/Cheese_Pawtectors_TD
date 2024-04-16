using UnityEngine;

public class DropCat : DragAndDropHandler
{
    [SerializeField] private Cat _currentCat;

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        //Debug.Log(_currentCat == null ? "Room is empty" : "Room is full");

        if (_currentCat != null)
        {
            base.HandleDragAndDrop(cat, initialPosition);
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
            cat.transform.parent.gameObject.GetComponent<DropCat>().ResetRoomSlot();
            InvokeOnRoomChanged((int)cat.transform.parent.transform.position.x, (int)cat.transform.parent.transform.position.y, -1);
        }

        InvokeOnRoomChanged((int)transform.position.x, (int)transform.position.y, cat.Level);

        _currentCat = cat;
        _currentCat.transform.SetParent(transform);
        _currentCat.transform.position = transform.position;
        //cat.transform.position = Camera.main.ScreenToWorldPoint(cat.transform.position);
    }

    public void ResetRoomSlot()
    {
        _currentCat = null;
    }
}