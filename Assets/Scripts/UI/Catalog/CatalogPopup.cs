using System;
using UnityEngine;
using UnityEngine.UI;

public class CatalogPopup : MonoBehaviour
{
    public static event Action<bool> OnToggle;
    public ScrollRect scrollRect;
    private string scrollPositionKey = "ScrollPosition";

    private void OnEnable()
    {
        OnToggle?.Invoke(true);
        AdoptButton.OnAdoptButtonClick += AdoptButton_OnAdoptButtonClick;
        // Load saved ScrollRect position on popup activation
        Vector2 savedPosition = new Vector2(PlayerPrefs.GetFloat(scrollPositionKey + "X", 0f),
                                            PlayerPrefs.GetFloat(scrollPositionKey + "Y", 0f));
        scrollRect.normalizedPosition = savedPosition;
    }

    private void OnDisable()
    {
        OnToggle?.Invoke(false);
        AdoptButton.OnAdoptButtonClick -= AdoptButton_OnAdoptButtonClick;
        // Save the current position of the ScrollRect when the popup is deactivated
        PlayerPrefs.SetFloat(scrollPositionKey + "X", scrollRect.normalizedPosition.x);
        PlayerPrefs.SetFloat(scrollPositionKey + "Y", scrollRect.normalizedPosition.y);
        PlayerPrefs.Save(); // Save changes in PlayerPrefs
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