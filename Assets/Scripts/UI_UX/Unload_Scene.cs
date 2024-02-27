using UnityEngine;
using UnityEngine.SceneManagement;

public class Unload_Scene : MonoBehaviour
{
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync("LoginPopup");
    }
}
