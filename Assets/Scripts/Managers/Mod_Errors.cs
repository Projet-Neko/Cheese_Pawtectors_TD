using UnityEngine;
using UnityEngine.SceneManagement;

public class Mod_Errors : MonoBehaviour
{
    [SerializeField] private string _errorPopup;

    public string ErrorMessage => _errorMessage;

    private string _errorMessage;

    private void Awake()
    {
        GameManager.OnError += ShowPopup;
    }

    private void OnDestroy()
    {
        GameManager.OnError -= ShowPopup;
    }

    private void ShowPopup(string errorMessage)
    {
        _errorMessage = errorMessage;
        SceneManager.LoadSceneAsync(_errorPopup, LoadSceneMode.Additive);
    }
}