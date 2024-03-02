using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField, Scene] private string _scene;

    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(_scene);
    }
}