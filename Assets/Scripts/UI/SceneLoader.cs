using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static event Action<bool, string> OnPopupSceneToogle;

    [SerializeField, Scene] private string _scene;

    private void Awake()
    {
        GameManager.OnInitComplete += GameManager_OnInitComplete;
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
        SceneManager.LoadScene(_scene);
    }

    public void LoadSceneAdditive()
    {
        if (GameManager.Instance.IsPopupSceneLoaded) UnloadSceneAdditive();
        SceneManager.LoadScene(_scene, LoadSceneMode.Additive);
        OnPopupSceneToogle?.Invoke(true, _scene);
    }

    public void UnloadSceneAdditive()
    {
        SceneManager.UnloadSceneAsync(GameManager.Instance.PopupSceneName);
        OnPopupSceneToogle?.Invoke(false, null);
    }
}