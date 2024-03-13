using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private DefaultPlayerActions _defaultPlayerActions;

    private InputAction _press;
    private InputAction _pressPosition;
    private InputAction _pressDelta;

    private MoveCameraAction _moveCameraAction;
    private bool _isMovingCamera = false;

    private DragAndDrop2 _dragAndDrop;
    private bool _isDragAndDrop = false;

    // private Swipe _swipe;
    private bool _isSwipe = false;

    private void Awake()
    {
        _defaultPlayerActions = new DefaultPlayerActions();

        _moveCameraAction = new MoveCameraAction();
    }

    // Enable Inputs
    private void OnEnable()
    {
        _pressPosition = _defaultPlayerActions.Player.PressPosition;
        _pressPosition.Enable();

        _pressDelta = _defaultPlayerActions.Player.PressDelta;
        _pressDelta.Enable();

        _press = _defaultPlayerActions.Player.Press;
        _press.started += StartPress;
        _press.canceled += StopPress;
        _press.Enable();
    }

    // Disable Inputs
    private void OnDisable()
    {
        _pressPosition.Disable();

        _pressDelta.Disable();

        _press.started -= StartPress;
        _press.canceled -= StopPress;
        _press.Disable();
    }

    // Event when the press is started
    private void StartPress(InputAction.CallbackContext context)
    {
        if (Input.touchCount > 1)
            return;

        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_pressPosition.ReadValue<Vector2>()), Vector2.zero);    // Get element clicked

        if (hit.collider)                                                                                                           // If there is a collider
        {
            switch (hit.collider.tag)                                                                                               // Check the tag for the action to realize and enable the action
            {
                case "MoveCamera":
                    _isMovingCamera = true;
                    StartCoroutine(MoveCamera());
                    break;
                case "DragAndDrop":
                    _isDragAndDrop = true;
                    StartCoroutine(DragAndDrop());
                    break;
                case "Swipe":
                    if (_pressDelta.ReadValue<Vector2>().magnitude > 0.5f)  // If the swipe is long enough
                    {
                        _isSwipe = true;
                        StartCoroutine(Swipe());
                    }
                    break;
                default:
                    Debug.Log("Default");
                    break;
            }
        }
    }

    // Event when the press is stopped
    private void StopPress(InputAction.CallbackContext context)
    {
        // Disable all actions
        _isMovingCamera = false;
        _isDragAndDrop = false;
        _isSwipe = false;

        Debug.Log("Stop");
    }

    private IEnumerator MoveCamera()
    {
        while (_isMovingCamera)
        {
            _moveCameraAction.UpdateMovement(_pressDelta.ReadValue<Vector2>());
            yield return null;
        }
    }

    private IEnumerator DragAndDrop()
    {
        while (_isDragAndDrop)
        {
            // _dragAndDrop.UpdateMovement(_pressDelta.ReadValue<Vector2>());
            Debug.Log("DragAndDrop");
            yield return null;
        }
    }

    private IEnumerator Swipe()
    {
        while (_isSwipe)
        {
            // _swipe.UpdateMovement(_pressDelta.ReadValue<Vector2>());
            Debug.Log("Swipe");
            yield return null;
        }
    }
}