using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _hud;
    [SerializeField] private Rigidbody2D _rbCat;
    [SerializeField] private Entity _entity;

    [SerializeField] private bool _isBeingDragged = false;
    [SerializeField] private GameObject _target;
    [SerializeField] private Transform _initialSlot;


    private void Start()
    {
        _initialSlot = transform.parent;
    }

    private void OnMouseDown()
    {
        Debug.Log("on mouse down");
        _isBeingDragged = true;
        _sprite.sortingOrder = 99;
        _hud.SetActive(false);
    }

    private void OnMouseDrag()
    {
        if (_isBeingDragged)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = new Vector3 (mousePosition.x, mousePosition.y, transform.position.z);
        }
    }

    private void OnMouseUp()
    {
        _isBeingDragged = false;
        _sprite.sortingOrder = 6;
        _hud.SetActive(true);

        if (_target == null)
        {
            BackSlot();
            return;
        }

        switch (_target.layer)
        {
            case 6: // Discard
                Destroy(gameObject);
                break;
            case 7: //merge chat
                Cat targetCat = _target.GetComponentInParent<Cat>();
                Debug.Log(targetCat.name);

                if (targetCat.Level == _entity.Level)
                {
                    targetCat.LevelUp();
                    Destroy(gameObject);
                }
                break;
            case 9: //Déplacement des chats dans d'autre slots
                if (_target.transform.childCount != 0) break;

                _initialSlot = _target.transform;
                transform.SetParent(_target.transform);
                transform.position = new Vector3(_target.transform.position.x, _target.transform.position.y, transform.position.z);
                break;
        }

        BackSlot();
    }

    private void BackSlot()
    {
        transform.SetParent(_initialSlot);
        transform.position = new Vector3(_initialSlot.transform.position.x, _initialSlot.transform.position.y, transform.position.z);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _target = collision.gameObject;
        //Debug.Log(_target.name);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _target) _target = null;
    }
}