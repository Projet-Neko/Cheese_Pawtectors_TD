using System;
using TMPro;
using UnityEngine;

public class SceneUI_LoginPopup : MonoBehaviour
{
    [SerializeField] private TMP_Text _offlineTime;
    [SerializeField] private TMP_Text _treatsNumber;

    private void Awake()
    {
        TimeSpan time = DateTime.UtcNow.Subtract(GameManager.Instance.LastLogin.Value);
        string formattedTime = time.ToString(@"hh\:mm\:ss");
        _offlineTime.text = formattedTime;
        _treatsNumber.text = $"+{GameManager.Instance.MeatGainedOffline()}";
    }
}