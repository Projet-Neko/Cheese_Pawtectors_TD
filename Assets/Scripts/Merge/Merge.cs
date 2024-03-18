using System;
using UnityEngine;

public class Merge : DragAndDropHandler
{
    public static event Action<int, int> OnCatMerge;

    [SerializeField] private Cat _cat;

    private int _slotIndex;
    private int _previousSlotIndex;

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        _slotIndex = int.Parse(transform.parent.name.Split('_')[1]);
        _previousSlotIndex = int.Parse(cat.transform.parent.name.Split('_')[1]);

        if (_cat.Level == cat.Level) MergeCat(cat);
        else base.HandleDragAndDrop(cat, initialPosition);
    }

    private void MergeCat(Cat cat)
    {
        OnCatMerge?.Invoke(_previousSlotIndex, -1);
        OnCatMerge?.Invoke(_slotIndex, _cat.Level);

        _cat.LevelUp();
        Destroy(cat.gameObject);
    }
}