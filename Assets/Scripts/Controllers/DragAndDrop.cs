using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [Header("Cat dependencies")]
    [SerializeField] private Cat _cat;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _hud;
    [SerializeField] private Collider2D _collider2D;

    [Header("Room dependencies")]
    [SerializeField] private Room _room;
    [SerializeField] private Collider _collider3D;

    public static event System.Action OnDragAndDrop;

    private bool _isBeingDragged = false;
    private Vector3 _initialPosition;
    private Vector3 _offset;

    private void OnMouseDrag()
    {
        if (_room != null && !_room.IsInStorageMode) return;
        if (!_isBeingDragged) return;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsPopupSceneLoaded) return;
        if (_room != null && !_room.IsInStorageMode) return;
        Grab(true);
        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseUp()
    {
        if (!_isBeingDragged) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit2D = Physics2D.GetRayIntersection(ray);

        if (Physics.Raycast(ray, out RaycastHit hit3D)) HandleRaycast(hit3D.collider.gameObject); // Raycast en 3D pour les pièces
        else if (hit2D.collider != null) HandleRaycast(hit2D.collider.gameObject); // Raycast en 2D pour l'entrepôt
        else transform.position = _initialPosition;
        
        Grab(false);
    }

    private void HandleRaycast(GameObject go)
    {
        Debug.Log("Objet touché : " + go.name);

        if (go.TryGetComponent(out DragAndDropHandler component))
        {
            if (_cat != null) component.HandleDragAndDrop(_cat, _initialPosition);
            else if (_room != null) component.HandleDragAndDrop(_room, _initialPosition);
        }
    }

    private void Grab(bool isGrabbed)
    {
        if (isGrabbed) _initialPosition = transform.position;
        _isBeingDragged = isGrabbed;
        if (_hud  != null) _hud.SetActive(!isGrabbed);
        if (_collider2D != null) _collider2D.enabled = !isGrabbed;
        if (_collider3D != null) _collider3D.enabled = !isGrabbed;
        if (_sprite != null) _sprite.sortingOrder = isGrabbed ? 99 : 6;

        if (_room != null) House.Instance.ActiveLine(isGrabbed);
        OnDragAndDrop?.Invoke();
    }
}