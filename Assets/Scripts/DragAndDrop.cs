using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rbCat;
    [SerializeField] private Entity _entity;

    private bool _isBeingDragged = false;
    private GameObject _target;
    //private Vector3 _initialSlot;
    private Transform _initialSlot;


    private void Start()
    {
        //_initialSlot = transform.position;
        _initialSlot = transform.parent;
    }

    private void OnMouseDown()
    {
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

        if (_target != null)
        {
            if (_target.layer == 6)
            {
                Destroy(gameObject);
            }

            else if (_target.layer == 7)
            {
                Cat targetCat = _target.GetComponent<Cat>();
                if (targetCat.Level != _entity.Level)
                {
                    targetCat.LevelUp();
                }
                Destroy(gameObject);

                _target.GetComponent<Cat>().LevelUp();
            }

            else if (_target.layer == 9)
            {
                transform.SetParent(_target.transform);
                transform.position = _target.transform.position;
            }
        }

        else 
        {
            //transform.position = _initialPosition;

            transform.SetParent(_initialSlot);
            transform.position = _initialSlot.position;
        } 
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _target = collision.gameObject;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject == _target) _target = null;
    }
}