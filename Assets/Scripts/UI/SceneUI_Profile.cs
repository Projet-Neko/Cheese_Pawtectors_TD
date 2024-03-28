using TMPro;
using UnityEngine;

public class SceneUI_Profile : MonoBehaviour
{
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_InputField _usernameTyped;

    private void Update()
    {
        _username.text = GameManager.Instance.Username;
    }

    public void EditUsername()
    {
        if (string.IsNullOrEmpty(_usernameTyped.text)) return; // TODO -> error feedback
        GameManager.Instance.UpdateUsername(_usernameTyped.text);
    }
}