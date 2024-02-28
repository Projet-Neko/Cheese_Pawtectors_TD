using UnityEngine;
using UnityEngine.SceneManagement;

public class UnloadScene : MonoBehaviour
{
    public void UnloadLoginPopupScene()
    {
        SceneManager.UnloadSceneAsync("LoginPopup");
    }

    public void UnloadSettingsScene()
    {
        SceneManager.UnloadSceneAsync("Settings");
    }
}
