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

        /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.position = new Vector3(mousePosition.x, mousePosition.y, transform.position.z);*/

        transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition) + _offset;
    }

    private Vector3 RoundPosition(Vector3 startPosition)
    {
        Vector3 position = startPosition;
        position.x = Mathf.Round(position.x);
        position.y = 0;
        position.z = Mathf.Round(position.z);

        return position;
    }

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsPopupSceneLoaded) return;
        _offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition); // Calcul de l'offset
        Grab(true);
    }

    private void OnMouseUp()
    {
        if (!_isBeingDragged) return;

        // Raycast en 3D pour les rooms
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit3D;

        // Raycast en 2D pour l'entrepôt
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = 60f; // Ajuster la coordonnée z à la position z du canvas
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit2D = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (Physics.Raycast(ray, out hit3D))
        {
            Debug.Log("Objet touché : " + hit3D.collider.gameObject.name);
        }
        else if (hit2D.collider != null)
        {
            Debug.Log("Objet touché : " + hit2D.collider.gameObject.name);
        }
        else transform.position = _initialPosition;

        /*Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = -1f;
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.TryGetComponent(out DragAndDropHandler component))
        {
            //Debug.Log($"raycast on {hit.collider.gameObject.name}");
            if (_cat != null) component.HandleDragAndDrop(_cat, _initialPosition);
            else if (_room != null) component.HandleDragAndDrop(_room, _initialPosition);
        }*/

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