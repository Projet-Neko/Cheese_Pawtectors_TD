using System;
using UnityEngine;

public class CatalogPopup : MonoBehaviour
{
    public static event Action<bool> OnToggle;

    private void OnEnable()
    {
        OnToggle?.Invoke(true);
        AdoptButton.OnAdoptButtonClick += AdoptButton_OnAdoptButtonClick;
    }

    private void OnDisable()
    {
        OnToggle?.Invoke(false);
        AdoptButton.OnAdoptButtonClick -= AdoptButton_OnAdoptButtonClick;
    }

    private void AdoptButton_OnAdoptButtonClick(int obj)
    {
        Debug.Log("on adopt button click");
        gameObject.SetActive(false);
    }

    public void ClosePopup()
    {
        gameObject.SetActive(false);
    }
}