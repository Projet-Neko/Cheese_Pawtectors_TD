//using TMPro;
//using UnityEngine;
//using UnityEngine.InputSystem;
//using UnityEngine.UI;
//using static UnityEngine.Rendering.DebugUI;

//public class SceneUI_Profile : MonoBehaviour
//{
//    [SerializeField] private TMP_Text _username;
//    [SerializeField] private TMP_InputField _usernameTyped;

//    private TouchScreenKeyboard _keyboard;

//    private void Update()
//    {
//        _username.text = GameManager.Instance.Username;
//    }

//    public void EditUsername()
//    {
//        if (string.IsNullOrEmpty(_usernameTyped.text)) return; // TODO -> error feedback
//        GameManager.Instance.UpdateUsername(_usernameTyped.text);
//    }

//    public void ModifyName()
//    {
//        // Open the native keyboard of the device
//        _keyboard = TouchScreenKeyboard.Open("", TouchScreenKeyboardType.Default, false, false, false, false, _usernameTyped.text);

//        // Subscribe to the onKeyboardClosed event
//        _keyboard.onKeyboardClosed += HandleKeyboardClosed;
//    }

//    private void HandleKeyboardClosed(TouchScreenKeyboard keyboard)
//    {
//        if (!string.IsNullOrEmpty(keyboard.text))
//        {
//            _usernameTyped.text = keyboard.text;
//            // Add your logic here to save the name in the player's profile
//        }

//        // Unsubscribe from the event to avoid memory leaks
//        keyboard.onKeyboardClosed -= HandleKeyboardClosed;

//        // Reset the keyboard variable
//        this._keyboard = null;
//    }
//}