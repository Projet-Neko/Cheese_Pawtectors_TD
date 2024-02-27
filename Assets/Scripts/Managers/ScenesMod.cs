using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesMod : MonoBehaviour
{

    private void Start()
    {
        SceneManager.LoadScene("LoginPopup", LoadSceneMode.Additive);
    }

    public void OnSettingsButtonClick()
    {
        SceneManager.LoadScene("Settings", LoadSceneMode.Additive);
    }
}
