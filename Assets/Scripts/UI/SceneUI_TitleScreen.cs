using UnityEngine;

public class SceneUI_TitleScreen : MonoBehaviour
{
    [SerializeField] private GameObject[] _titleScreenButtons;
    [SerializeField] private GameObject _checkingText;
    [SerializeField] private GameObject _loadingGO;

    private void Awake()
    {
        Mod_Account.OnLocalDataCheck += Mod_Account_OnLocalDataCheck;
        Mod_Account.OnLoginStart += ShowLoadingBar;
    }

    private void OnDestroy()
    {
        Mod_Account.OnLocalDataCheck -= Mod_Account_OnLocalDataCheck;
        Mod_Account.OnLoginStart -= ShowLoadingBar;
    }

    private void Mod_Account_OnLocalDataCheck(bool hasLocalData)
    {
        _checkingText.SetActive(false);

        if (hasLocalData) return;
        else foreach (var button in _titleScreenButtons) button.SetActive(true);
    }

    public void HideButtons()
    {
        foreach (var button in _titleScreenButtons) button.SetActive(false);
    }

    private void ShowLoadingBar()
    {
        _loadingGO.gameObject.SetActive(true);
    }
}