using UnityEngine;

public class StorageSlot : DragAndDropHandler
{
    [SerializeField] private Cat _currentCat;
    [SerializeField] private BoxCollider2D _collider;

    private int _slotIndex;
    private int _previousSlotIndex;

    private void Awake()
    {
        _slotIndex = int.Parse(name.Split('_')[1]);
    }

    private void Update()
    {
        _collider.enabled = _currentCat == null ? true : false;
    }

    public void InitSlot(Cat cat)
    {
        _currentCat = cat;
    }

    public override void HandleDragAndDrop(Cat cat, Vector3 initialPosition)
    {
        if (transform.childCount == 0) _currentCat = null;
        _previousSlotIndex = int.Parse(cat.transform.parent.name.Split('_')[1]);
        //Debug.Log(_currentCat == null ? "No current cat" : _currentCat.gameObject.name);

        if (_currentCat == null) MoveCat(cat);
        else if (_currentCat.Level == cat.Level) MergeCat(cat);
        else base.HandleDragAndDrop(cat, initialPosition);
    }

    private void MoveCat(Cat cat)
    {
        InitSlot(cat);

        InvokeOnSlotChanged(_previousSlotIndex, -1);
        InvokeOnSlotChanged(_slotIndex, _currentCat.Level - 1);

        _currentCat.transform.SetParent(transform);
        _currentCat.transform.position = transform.position;

        //_currentCat.transform.position = new Vector3(transform.position.x, transform.position.y - 0.1f, transform.position.z + 0.1f);

        Debug.Log($"Position du slot : {transform.position}");
        Debug.Log($"Position du chat : {_currentCat.transform.position}");
    }

    private void MergeCat(Cat cat)
    {
        InvokeOnSlotChanged(_previousSlotIndex, -1);
        InvokeOnSlotChanged(_slotIndex, _currentCat.Level);

        _currentCat.LevelUp();
        Destroy(cat.gameObject);
    }
}