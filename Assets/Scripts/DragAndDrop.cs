using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rbCat;
    [SerializeField] private Cat cat;

    private bool _isBeingDragged = false;
    private GameObject _target;

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
                cat.LevelUp();

                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        _target = collision.gameObject;
    }
}
