using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
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
    }

    private void OnMouseDrag()
    {
        if (_isBeingDragged)
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = transform.position.z;
            transform.position = mousePosition;
        }
    }

    private void OnMouseUp()
    {
        _isBeingDragged = false;

        if (_target == null)
        {
            BackSlot();
            return;
        }

        switch (_target.layer)
        {
            case 6:
                Destroy(gameObject);
                break;
            case 7:
                Cat targetCat = _target.GetComponentInParent<Cat>();
                Debug.Log(targetCat.name);

                if (targetCat.Level == _entity.Level)
                {
                    targetCat.LevelUp();
                    Destroy(gameObject);
                }
                break;
            case 9:
                if (_target.transform.childCount != 0) break;

                _initialSlot = _target.transform;
                transform.SetParent(_target.transform);
                transform.position = _target.transform.position;
                break;
        }

        BackSlot();
    }

    private void BackSlot()
    {
        transform.SetParent(_initialSlot);
        transform.position = _initialSlot.position;
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