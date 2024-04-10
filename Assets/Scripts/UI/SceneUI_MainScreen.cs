using NaughtyAttributes;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUI_MainScreen : MonoBehaviour
{
    [SerializeField, Scene] private string _sceneHeadBand;
    [SerializeField, Scene] private string _sceneLoginPopup;

    void Start()
    {
        SceneManager.LoadScene(_sceneHeadBand, LoadSceneMode.Additive);
        if (!GameManager.Instance.HasLoginPopupLoad()) SceneManager.LoadScene(_sceneLoginPopup, LoadSceneMode.Additive);
    }
}