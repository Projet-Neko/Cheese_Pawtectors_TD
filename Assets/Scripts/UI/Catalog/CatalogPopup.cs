using System;
using UnityEngine;

public class CatalogPopup : MonoBehaviour
{
    public static event Action<bool> OnToggle;

    private void OnEnable()
    {
        OnToggle?.Invoke(true);
    }

    private void OnDisable()
    {
        OnToggle?.Invoke(false);
    }

    public void Buy()
    {
        Close();
    }

    public void Close()
    {
        //OnToggle?.Invoke(false);
        gameObject.SetActive(false);
    }
}