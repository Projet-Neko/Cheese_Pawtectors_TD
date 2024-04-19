using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, Scene] protected string _scene;
    [SerializeField] private bool _isAdditive = true;

    private LoadSceneMode _loadSceneMode;

    private void Awake()
    {
        GameManager.OnInitComplete += LoadScene;
        _loadSceneMode = _isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
    }

    private void OnDestroy()
    {
        GameManager.OnInitComplete -= LoadScene;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(_scene, _loadSceneMode);
    }
}