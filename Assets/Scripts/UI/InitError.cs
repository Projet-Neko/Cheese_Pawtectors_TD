using TMPro;
using UnityEngine;

public class InitError : MonoBehaviour
{
    [SerializeField] private TMP_Text _errorMessage;

    private void Start()
    {
        Debug.LogWarning(GameManager.Instance.ErrorMessage);
        _errorMessage.text = GameManager.Instance.ErrorMessage;
    }
}