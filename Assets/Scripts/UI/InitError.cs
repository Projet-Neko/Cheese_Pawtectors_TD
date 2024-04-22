using TMPro;
using UnityEngine;

public class InitError : MonoBehaviour
{
    [SerializeField] private TMP_Text _errorMessage;

    private void Awake()
    {
        _errorMessage.text = GameManager.Instance.ErrorMessage;
    }
}