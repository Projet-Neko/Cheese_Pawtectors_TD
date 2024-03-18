using NaughtyAttributes;
using System;
using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _hud;
    [SerializeField] private Cat _cat;
    [SerializeField] private Room _room;

    [Header("Layers")]
    [SerializeField, Layer] private int _catLayer;
    [SerializeField, Layer] private int _discardLayer;
    [SerializeField, Layer] private int _slotLayer;

    private bool _isBeingDragged = false;
    private GameObject _target;
    private Vector3 _initialPosition;

    private void OnMouseDrag()
    {
        if (!_isBeingDragged) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = transform.position.z;
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    private void OnMouseDown()
    {
        Grab(true);
    }

    private void OnMouseUp()
    {
        Grab(false);

        if (_target == null) transform.position = _initialPosition;
        else if (_target.TryGetComponent(out DragAndDropHandler component))
        {
            Debug.Log("get drag and drop handler");
            if (_cat != null) component.HandleDragAndDrop(_cat, _initialPosition);
            else if (_room != null) component.HandleDragAndDrop(_room, _initialPosition);
        }
        else transform.position = _initialPosition;
    }

    private void Grab(bool isGrabbed)
    {
        if (isGrabbed) _initialPosition = transform.position;
        _isBeingDragged = isGrabbed;
        _hud.SetActive(!isGrabbed);
        _sprite.sortingOrder = isGrabbed ? 99 : 6;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_isBeingDragged) return;
        _target = collision.gameObject;
        Debug.Log($"Targeting {_target.name}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!_isBeingDragged) return;

        if (collision.gameObject == _target)
        {
            _target = null;
            Debug.Log("Remove target");
        }

        // TODO -> parfois la target se retire alors que le trigger n'est pas exit
    }
}