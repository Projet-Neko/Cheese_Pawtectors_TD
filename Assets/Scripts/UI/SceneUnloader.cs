using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}