using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _hud;
    [SerializeField] private Cat _cat;
    [SerializeField] private Room _room;
    [SerializeField] private Collider2D _collider;

    private bool _isBeingDragged = false;
    private Vector3 _initialPosition;

    private void OnMouseDrag()
    {
        if (!_isBeingDragged) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsPopupSceneLoaded) return;
        Grab(true);
    }

    private void OnMouseUp()
    {
        if (!_isBeingDragged) return;

        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -1f;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out DragAndDropHandler component))
        {
            //Debug.Log($"raycast on {hit.collider.gameObject.name}");
            if (_cat != null) component.HandleDragAndDrop(_cat, _initialPosition);
            else if (_room != null) component.HandleDragAndDrop(_room, _initialPosition);
        }
        else transform.position = _initialPosition;

        Grab(false);
    }

    private void Grab(bool isGrabbed)
    {
        if (isGrabbed) _initialPosition = transform.position;
        _isBeingDragged = isGrabbed;
        _hud.SetActive(!isGrabbed);
        _collider.enabled = isGrabbed ? false : true;
        _sprite.sortingOrder = isGrabbed ? 99 : 6;
    }
}