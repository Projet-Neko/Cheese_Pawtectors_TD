public class SceneLoader_LoginPopup : SceneLoader
{
    private void Start()
    {
        if (!GameManager.Instance.HasLoginPopupLoad()) LoadSceneAdditive();
    }
}