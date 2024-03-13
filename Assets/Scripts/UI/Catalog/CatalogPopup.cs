using System;
using UnityEngine;

public class CatalogPopup : MonoBehaviour
{
    public static event Action<bool> OnToggle;

    private void OnEnable()
    {
        Debug.Log("enable");
        OnToggle?.Invoke(true);
    }

    public void Close()
    {
        Debug.Log("close");
        OnToggle?.Invoke(false);
        gameObject.SetActive(false);
    }
}