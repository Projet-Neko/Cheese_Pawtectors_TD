using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static event Action<bool, string> OnPopupSceneToogle;

    [SerializeField, Scene] protected string _scene;
    [SerializeField] private bool _isAdditive;
    private LoadSceneMode _loadSceneMode;

    private void Awake()
    {
        GameManager.OnInitComplete += GameManager_OnInitComplete;
        _loadSceneMode = _isAdditive ? LoadSceneMode.Additive : LoadSceneMode.Single;
    }

    private void OnDestroy()
    {
        GameManager.OnInitComplete -= GameManager_OnInitComplete;
    }

    private void GameManager_OnInitComplete()
    {
        LoadScene();
    }

    public void LoadScene()
    {
        if (_isAdditive && GameManager.Instance.IsPopupSceneLoaded) UnloadScene(GameManager.Instance.PopupSceneName);
        SceneManager.LoadScene(_scene, _loadSceneMode);
        if (!_isAdditive) return;
        //OnPopupSceneToogle?.Invoke(true, _scene);
    }

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(_scene);
        //OnPopupSceneToogle?.Invoke(false, null);
    }

    private void UnloadScene(string scene)
    {
        SceneManager.UnloadSceneAsync(scene);
        //OnPopupSceneToogle?.Invoke(false, null);
    }
}