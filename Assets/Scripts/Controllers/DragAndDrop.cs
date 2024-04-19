using System.ComponentModel;
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
    private Vector3 _offset;

    private void OnMouseDrag()
    {
        if (!_isBeingDragged) return;
        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsPopupSceneLoaded) return;
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
        //Debug.Log("Objet touché : " + go.name);

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
        _hud.SetActive(!isGrabbed);
        _collider.enabled = isGrabbed ? false : true;
        _sprite.sortingOrder = isGrabbed ? 99 : 6;
    }
}