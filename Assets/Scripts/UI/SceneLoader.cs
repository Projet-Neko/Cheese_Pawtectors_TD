using NaughtyAttributes;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static event Action<bool> OnPopupSceneToogle;

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
        OnPopupSceneToogle?.Invoke(true);
        SceneManager.LoadScene(_scene, LoadSceneMode.Additive);
    }

    public void UnloadSceneAdditive()
    {
        OnPopupSceneToogle?.Invoke(false);
    }
}