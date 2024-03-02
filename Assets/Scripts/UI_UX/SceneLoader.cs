using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField, Scene] private string _scene;

    public void LoadScene()
    {
        SceneManager.LoadScene(_scene);
    }

    public void LoadSceneAdditive()
    {
        SceneManager.LoadScene(_scene, LoadSceneMode.Additive);
    }
}