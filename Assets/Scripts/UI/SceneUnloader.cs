using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneUnloader : MonoBehaviour
{
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(GameManager.Instance.PopupSceneName);
    }
}