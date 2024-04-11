using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum RoomPattern
{
    CheeseRoom,         // Moved
    CorridorRoom,       // MovedAndRemoved
    IntersectionRoom,   // MovedAndRemoved
    CrossraodRoom,      // MovedAndRemoved
    StartRoom,          // Protected
    TurnRoom,           // MovedAndRemoved
    VoidRoom            // Overwritten
}

public enum RoomSecurity
{
    Protected,          // The room can't be moved or removed
    Moved,              // The room can be moved
    MovedAndRemoved,    // The room can be moved and removed
    Overwritten         // A room can be placed over this one
}

public struct IdRoom
{
    public int x;
    public int z;

    public IdRoom(int xRoom, int yRoom)
    {
        x = xRoom;
        z = yRoom;
    }

    public bool IsNull()
    {
        return x < 0 || z < 0;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        IdRoom id = (IdRoom)obj;
        return x == id.x && z == id.z;
    }

    public override int GetHashCode() { return HashCode.Combine(x, z); }
}

public class Room : MonoBehaviour
{
    [Header("Room")]
    [SerializeField] private GameObject _room;

    [Header("Room Canva")]
    [SerializeField] private GameObject _HUDCanva;
    [SerializeField] private GameObject _moveModCanva;
    [SerializeField] GameObject _cantMoveThisRoomCanvas;

    [Header("Junction")]
    [SerializeField] protected List<Junction> _opening;

    // Events
    public static event Action<Vector3, Vector3, bool> ChangeTilePosition;  // Old position, new position, Still in motion or not (false if the room is still moving)
    public static event Action<int, int, RoomPattern> TileDestroyed;        // Notify the house that a room is destroyed

    // Getters
    public List<Junction> Opening => _opening;
    public bool CorrectPath => _correctPath;
    public RoomSecurity Security => _security;
    public List<IdRoom> PreviousRooms => _previousRooms;
    public List<IdRoom> NextRooms => _nextRooms;

    protected RoomSecurity _security;
    protected bool _correctPath = false;                                    // True if the room is in a correct path

    // Events
    private static event Action<bool> TileSelected;                         // Deselect the other rooms when a room is selected

    private Vector3 _oldPosition;
    private bool _canMove;
    private bool _moveModBool;
    private bool _isSelected;
    protected bool _anotherTileSelected;
    private int _currentLevel = 1;

    private List<IdRoom> _previousRooms = new List<IdRoom>();
    private List<IdRoom> _nextRooms = new List<IdRoom>();

    private string _sceneHUD;

    // Constants
    private const int _maxLevel = 3;
    private Plane _plane = new Plane(Vector3.up, 0);


    //change OnMouseDown to Button to avoid click error
    //Link delete button to delete function of house


    /* * * * * * * * * * * * * * * * * * * *
     *          BASIC FUNCTIONS
     * * * * * * * * * * * * * * * * * * * */

    private void OnDestroy()
    {
        // Unsubscribe from events
        TileSelected -= DeselectTile;
    }
    
    void Start()
    {
        // Subscribe to events
        TileSelected += DeselectTile;

        _canMove = false;
        _moveModBool = false;
    }

    private void FixedUpdate()
    {
        if (_moveModBool && _canMove)
        {
            float distance;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (_plane.Raycast(ray, out distance))
            {
                Vector3 mousePosition = ray.GetPoint(distance);
                _room.transform.position = RoundPosition(mousePosition);
            }
        }
    }

    /* * * * * * * * * * * * * * * * * * * *
     *          UTILITIES FUNCTIONS
     * * * * * * * * * * * * * * * * * * * */

    private Vector3 RoundPosition(Vector3 startPosition)
    {
        Vector3 position = startPosition;
        position.x = Mathf.Round(position.x);                        
        position.y = 0;                                // To be sure the room is on the same line
        position.z = Mathf.Round(position.z);

        return position;
    }

    /* * * * * * * * * * * * * * * * * * * *
     *          INPUT FUNCTIONS
     * * * * * * * * * * * * * * * * * * * */

    public void SceneForHUD(string scene)
    {
        _sceneHUD = scene;
    }

    public void OnMouseDown()
    {
       if(_sceneHUD == SceneManager.GetActiveScene().name)
        {
            if (_security == RoomSecurity.Protected)
            {
                _cantMoveThisRoomCanvas.SetActive(!_cantMoveThisRoomCanvas.activeSelf);
            }
            else if (!_anotherTileSelected)
            {
                _canMove = true;

                if (!_moveModBool)
                    Selected();
            }
        }
    }

    public void OnMouseUp()
    {
        _canMove = false;
    }

    private void Selected()
    {
        _isSelected = !_isSelected;
        _HUDCanva.SetActive(_isSelected);
        TileSelected?.Invoke(_isSelected); // Invoke the event to deselect the other rooms
    }

    private void DeselectTile(bool deselect)
    {
        if (!_isSelected) _anotherTileSelected = deselect;
    }

    /* * * * * * * * * * * * * * * * * * * *
     *            UI FUNCTIONS
     * * * * * * * * * * * * * * * * * * * */

    public void ShowUI()
    {
        _HUDCanva.SetActive(true);
    }

    /****** MOVE ROOM ******/

    // When user click on Canvas/HUD/Move button
    public void Move()
    {
        _moveModBool = true;
        _HUDCanva.SetActive(false);                 // Hide the HUD
        _moveModCanva.SetActive(true);              // Show the Move Canvas
        _oldPosition = _room.transform.position;    // Save the currently position
    }

    public void CancelMove()
    {
        _moveModBool = false;
        _moveModCanva.SetActive(false);                                                 // Hide the Move Canvas
        _isSelected = false;
        _room.transform.position = _oldPosition;
        TileSelected?.Invoke(false); 

    }

    // When user click on Canvas/Move Arrow/Done button
    public void StopMove()
    {
        _moveModBool = false;

        _moveModCanva.SetActive(false);                                                 // Hide the Move Canvas
        ChangeTilePosition?.Invoke(_oldPosition, _room.transform.position, true);       //true because the room ask for position validation
        TileSelected?.Invoke(false); // Invoke the event to deselect the other rooms
        _isSelected = false;

    }

    /*private void ChangePosition(bool validate)
    {

        if (_moveModBool)
        {
            if (validate)
            {
                //change Material to green
            }
            else
            {
                //change Material to red
            }
        }

        if (_WaitForValidation)
        {
            MoveRoomOldPosition();
            //change Material to normal           
            _WaitForValidation = false;
        }
    }*/

    public void MoveRoom(int x, int z)
    {
        _room.transform.position = new Vector3(x, 0, z);
    }

    public void MoveRoomOldPosition()
    {
        _room.transform.position = _oldPosition;
    }

    /****** ROTATE ROOM ******/

    // When user click on Canvas/HUD/Rotate [Clock/AntiClock]
    public void RotationRoom(bool clockwise) 
    {
        Vector3 rotation = transform.eulerAngles;

        if (clockwise)
        {
            rotation.y -= 90;

            if (rotation.y >= 360) rotation.y -= 360;

        }
        else
        {
            rotation.y += 90;

            if (rotation.y < 0) rotation.y += 360;
        }

        transform.eulerAngles = rotation;
    }

    /****** REMOVE ROOM ******/

    // When user click on Canvas/HUD/Suppr button
    public void Remove()
    {
        TileDestroyed?.Invoke((int)transform.position.x, (int)transform.position.z, RoomPattern.VoidRoom); // Notify the house that a room will be destroyed and that it must be replaced by a void room
        TileSelected?.Invoke(false);
        Delete();
    }

    public void Delete()
    {
        Destroy(_room);
    }


    /* * * * * * * * * * * * * * * * * * * *
    *        VALIDATION OF THE PATH
    * * * * * * * * * * * * * * * * * * * */

    public void ResetPath()
    {
        _correctPath = false;
        _previousRooms.Clear();
        _nextRooms.Clear();
    }

    public void ValidatePath()
    {
        _correctPath = true;
    }

    public void DefineIdRoom(int x, int z)
    {
        foreach (Junction junction in _opening)
            junction.SetIdRoom(x, z);
    }

    public void ResetArrows()
    {
        foreach (Junction junction in _opening)
            junction.ActivateArrow(false);
    }


    /* * * * * * * * * * * * * * * * * * * *
     *          MANAGE ROOM LEVEL
     * * * * * * * * * * * * * * * * * * * */

    private void LevelUp()
    {
        if (_currentLevel < _maxLevel)
            ++_currentLevel;
    }
}