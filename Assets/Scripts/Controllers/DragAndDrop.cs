using NaughtyAttributes;
using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    public static event Action<int, int> OnSlotChanged;

    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _hud;
    [SerializeField] private Entity _entity;

    [Header("Layers")]
    [SerializeField, Layer] private int _catLayer;
    [SerializeField, Layer] private int _discardLayer;
    [SerializeField, Layer] private int _slotLayer;

    public int CurrentSlotIndex => _currentSlotIndex;

    private bool _isBeingDragged = false;
    private GameObject _target;
    private Transform _currentSlot;
    private int _currentSlotIndex;

    // TODO -> update Data storage on cat move

    private void Start()
    {
        _currentSlot = transform.parent;
        _currentSlotIndex = int.Parse(_currentSlot.name.Split('_')[1]);
    }

    private void OnMouseDrag()
    {
        if (_isBeingDragged)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    private void OnMouseDown()
    {
        Grab(true);
    }

    private void OnMouseUp()
    {
        Grab(false);

        if (_target == null) BackSlot();
        else if (_target.layer == _catLayer) Merge();
        else if (_target.layer == _discardLayer) Destroy(gameObject);
        else if (_target.layer == _slotLayer) ChangeSlot();
        else BackSlot();
    }

    private void Grab(bool isGrabbed)
    {
        _isBeingDragged = isGrabbed;
        _hud.SetActive(!isGrabbed);
        _sprite.sortingOrder = isGrabbed ? 99 : 6;
    }

    private void Merge()
    {
        Cat target;

        if (_target.layer == _slotLayer)
        {
            target = _target.GetComponentInChildren<Cat>();
        }
        else target = _target.GetComponentInParent<Cat>();

        if (target.Level == _entity.Level)
        {
            OnSlotChanged?.Invoke(_currentSlotIndex, -1);
            OnSlotChanged?.Invoke(target.GetComponentInParent<DragAndDrop>().CurrentSlotIndex, _entity.Level);
            target.LevelUp();
            Destroy(gameObject);
            return;
        }

        BackSlot();
    }

    private void ChangeSlot()
    {
        if (_target.transform.childCount != 0)
        {
            Merge();
            return;
        }

        _currentSlot = _target.transform;
        _currentSlotIndex = int.Parse(_currentSlot.name.Split('_')[1]);
        transform.SetParent(_target.transform);
        transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
        OnSlotChanged?.Invoke(_currentSlotIndex, _entity.Level - 1);
    }

    private void BackSlot()
    {
        transform.SetParent(_currentSlot);
        transform.position = new Vector3(_currentSlot.transform.position.x, _currentSlot.transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _target = collision.gameObject;
        //Debug.Log($"Targeting {_target.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _target) _target = null;
        //Debug.Log("No target");
    }
}