using System;
using UnityEngine;

public class Discard : DragAndDropHandler
{
    public static event Action<int, int> OnCatDiscard; // slot index, empty index for cat

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        int previousSlotIndex = int.Parse(cat.transform.parent.name.Split('_')[1]);
        OnCatDiscard?.Invoke(previousSlotIndex, -1);
        Destroy(cat.gameObject);
    }
}