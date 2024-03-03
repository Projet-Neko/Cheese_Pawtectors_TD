using UnityEngine;

public class PlayerController : MonoBehaviour
{
    //private DefaultPlayerActions _defaultPlayerActions;

    //private InputAction _press;

    //private InputAction _inputPosition;
    //private InputAction _inputDelta;

    private MoveCameraAction _moveCameraAction;
    private bool _isMovingCamera = false;

    // private DragAndDrop _dragAndDrop;
    private bool _isDragAndDrop = false;

    // private Swipe _swipe;
    private bool _isSwipe = false;

    //private void Awake()
    //{
    //    _defaultPlayerActions = new DefaultPlayerActions();

    //    _moveCameraAction = new MoveCameraAction();
    //}

    // Enable Inputs
    //private void OnEnable()
    //{
    //    _inputPosition = _defaultPlayerActions.Player.InputPosition;
    //    _inputPosition.Enable();

    //    _inputDelta = _defaultPlayerActions.Player.InputDelta;
    //    _inputDelta.Enable();

    //    _press = _defaultPlayerActions.Player.Press;
    //    _press.started += StartPress;
    //    _press.canceled += StopPress;
    //    _press.Enable();
    //}

    // Disable Inputs
    //private void OnDisable()
    //{
    //    _inputPosition.Disable();

    //    _inputDelta.Disable();

    //    _press.started -= StartPress;
    //    _press.canceled -= StopPress;
    //    _press.Disable();
    //}

    // Event when the press is started
    //private void StartPress(InputAction.CallbackContext context)
    //{
    //    RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(_inputPosition.ReadValue<Vector2>()), Vector2.zero);    // Get element clicked

    //    if (hit.collider)                                                                                                           // If there is a collider
    //    {
    //        switch (hit.collider.tag)                                                                                               // Check the tag for the action to realize and enable the action
    //        {
    //            case "MoveCamera":
    //                _isMovingCamera = true;
    //                Debug.Log("MoveCamera");
    //                break;
    //            case "DragAndDrop":
    //                _isDragAndDrop = true;
    //                Debug.Log("DragAndDrop");
    //                break;
    //            case "Swipe":
    //                if (_inputDelta.ReadValue<Vector2>().magnitude > 0.5f)  // If the swipe is long enough
    //                    _isSwipe = true;
    //                Debug.Log("Swipe");
    //                break;
    //            default:
    //                Debug.Log("Default");
    //                break;
    //        }
    //    }
    //}

    // Event when the press is stopped
    //private void StopPress(InputAction.CallbackContext context)
    //{
    //    // Disable all actions
    //    _isMovingCamera = false;
    //    _isDragAndDrop = false;
    //    _isSwipe = false;

    //    Debug.Log("Stop");
    //}

    //private void FixedUpdate()
    //{
    //    if (_isMovingCamera)
    //        _moveCameraAction.UpdateMovement(_inputDelta.ReadValue<Vector2>());
    //    else if (_isDragAndDrop)
    //        // _dragAndDrop.UpdateMovement(_inputDelta.ReadValue<Vector2>());
    //        Debug.Log("DragAndDrop");
    //    else if (_isSwipe)
    //        // _swipe.UpdateMovement(_inputDelta.ReadValue<Vector2>());
    //        Debug.Log("Swipe");
    //}
}