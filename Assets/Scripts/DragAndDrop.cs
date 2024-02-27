using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rbCat;

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

        LayerMask lm = new() { value = 6 };
        ContactFilter2D cf = new() { layerMask = lm, useTriggers = true };

        if (_rbCat.IsTouching(cf)) Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 7) _target = collision.gameObject;
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (true)
    //    {

    //    }
    //}
}