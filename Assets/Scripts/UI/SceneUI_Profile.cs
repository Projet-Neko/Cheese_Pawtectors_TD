using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI_Profile : MonoBehaviour
{
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_InputField _usernameTyped;
    [SerializeField] private GameObject _clanCanvas;
    [SerializeField] private GameObject _clanPanel;
    [SerializeField] private TMP_Text _clanName;
    [SerializeField] private TMP_Text _clanDescription;
    [SerializeField] private Image _clanGem;

    private void Start()
    {
        if (GameManager.Instance.Data.Clan == -1) _clanCanvas.SetActive(true);
        else
        {
            Debug.Log($"Clan is {GameManager.Instance.Data.Clan}");
            _clanPanel.SetActive(true);
            int clan = GameManager.Instance.Data.Clan;
            _clanName.text = ((Clan)clan).ToString();
            _clanDescription.text = GameManager.Instance.Clans[clan].Description;
            _clanGem.sprite = GameManager.Instance.Clans[clan].Gem;
        }
    }

    private void Update()
    {
        _username.text = GameManager.Instance.Username;
    }

    public void EditUsername()
    {
        if (string.IsNullOrEmpty(_usernameTyped.text)) return; // TODO -> error feedback
        GameManager.Instance.UpdateUsername(_usernameTyped.text);
    }

    public void ModifyName()
    {
        _usernameTyped.ActivateInputField();
    }
}