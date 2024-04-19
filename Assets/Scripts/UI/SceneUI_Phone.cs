using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUI_Phone : MonoBehaviour
{
    [SerializeField, Scene] private string _sceneHeadBand;

    private void Start()
    {
        SceneManager.UnloadSceneAsync(_sceneHeadBand);
    }
}