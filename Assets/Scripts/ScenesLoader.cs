using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScenesLoader : MonoBehaviour
{
    [Scene]
    public string Scene;

    public void OnButtonClickAdd()
    {
        SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
    }

    public void OnButtonClick()
    {
        SceneManager.LoadScene(Scene);
    }
}
