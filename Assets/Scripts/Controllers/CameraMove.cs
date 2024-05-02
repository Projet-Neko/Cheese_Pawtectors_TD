using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    private Vector3 _touchStart;
    private float _zoomOutMin = 2;
    private float _zoomOutMax = 7;
    private bool _canMove = true;

    private void Awake()
    {
        RoomDrop.OnCatMoving += CanMove;
        Room.OnMovingRoom += CanMove;
        DragAndDrop.OnDragAndDrop += CanMove;
    }

    private void OnDestroy()
    {
        RoomDrop.OnCatMoving -= CanMove;
        Room.OnMovingRoom -= CanMove;
        DragAndDrop.OnDragAndDrop -= CanMove;
    }

    private void Update()
    {
        if (!_canMove) return;
        if (Input.GetMouseButtonDown(0))
        {
            _touchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            Zoom(difference * 0.01f);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector3 direction = _touchStart - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Camera.main.transform.position += direction;
        }
    }

    private void CanMove()
    {
        _canMove = !_canMove;
    }

    private void Zoom(float increment)
    {
        Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize - increment, _zoomOutMin, _zoomOutMax);
    }
}
