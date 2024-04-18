using UnityEngine;

public class StorageSlot : DragAndDropHandler
{
    [SerializeField] private Cat _currentCat;
    [SerializeField] private BoxCollider2D _collider;

    private int _slotIndex;

    private void Awake()
    {
        _slotIndex = int.Parse(name.Split('_')[1]);
    }

    private void Update()
    {
        if (transform.childCount == 0) _currentCat = null;
        _collider.enabled = _currentCat == null ? true : false;
    }

    public void InitSlot(Cat cat)
    {
        _currentCat = cat;
    }

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        //Debug.Log(_currentCat == null ? $"{name} is empty" : $"{name} is full with {_currentCat.gameObject.name}");

        if (_currentCat == null) MoveCat(cat);
        else if (_currentCat.Level == cat.Level) MergeCat(cat);
        else base.HandleDragAndDrop(cat, initialPosition);
    }

    private void MoveCat(Cat cat)
    {
        InitSlot(cat);
        ResetLastSpot(cat);

        InvokeOnSlotChanged(_slotIndex, _currentCat.Level - 1);

        _currentCat.transform.SetParent(transform);
        _currentCat.transform.position = transform.position;

        //_currentCat.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z + 0.1f);

        //Debug.Log($"Position du slot : {transform.position}");
        //Debug.Log($"Position du chat : {_currentCat.transform.position}");
    }

    private void MergeCat(Cat cat)
    {
        ResetLastSpot(cat);

        InvokeOnSlotChanged(_slotIndex, _currentCat.Level);

        _currentCat.LevelUp();
        Destroy(cat.gameObject);
    }

    private void ResetLastSpot(Cat cat)
    {
        if (!cat.IsInStorageMode)
        {
            cat.transform.parent.gameObject.GetComponent<DropCat>().ResetRoomSlot();
            InvokeOnRoomChanged((int)cat.transform.parent.transform.position.x, (int)cat.transform.parent.transform.position.y, -1);
            cat.SetStorageMode(true);
        }
        else
        {
            int previousSlotIndex = int.Parse(cat.transform.parent.name.Split('_')[1]);
            InvokeOnSlotChanged(previousSlotIndex, -1);
        }
    }
}