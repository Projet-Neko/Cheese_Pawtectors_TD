using UnityEngine;

public class TitleScreenUI : MonoBehaviour
{
    [SerializeField] private GameObject[] _titleScreenButtons;
    [SerializeField] private GameObject _checkingText;

    private void Awake()
    {
        Mod_Account.OnLocalDataCheck += Mod_Account_OnLocalDataNotFound;
    }

    private void OnDestroy()
    {
        Mod_Account.OnLocalDataCheck -= Mod_Account_OnLocalDataNotFound;
    }

    private void Mod_Account_OnLocalDataNotFound(bool hasLocalData)
    {
        _checkingText.SetActive(false);
        if (hasLocalData) return;
        foreach (var button in _titleScreenButtons) button.SetActive(true);
    }

    public void HideButtons()
    {
        foreach (var button in _titleScreenButtons) button.SetActive(false);
    }
}