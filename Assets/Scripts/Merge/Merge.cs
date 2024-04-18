using System;
using UnityEngine;

public class Merge : DragAndDropHandler
{
    public static event Action<int, int> OnCatMerge; // slot index, cat index

    [SerializeField] private Cat _cat;

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        if (_cat.Level == cat.Level) MergeCat(cat);
        else base.HandleDragAndDrop(cat, initialPosition);
    }

    private void MergeCat(Cat cat)
    {
        if (_cat.IsInStorageMode)
        {
            int slotIndex = int.Parse(transform.parent.name.Split('_')[1]);
            OnCatMerge?.Invoke(slotIndex, _cat.Level);
        }
        else
        {
            // Save current room data
        }

        if (cat.IsInStorageMode)
        {
            int previousSlotIndex = int.Parse(cat.transform.parent.name.Split('_')[1]);
            OnCatMerge?.Invoke(previousSlotIndex, -1);
        }
        else
        {
            // Save previous room data
        }

        _cat.LevelUp();
        Destroy(cat.gameObject);
    }
}