using System;
using UnityEngine;

public class CatBoxOpening : MonoBehaviour
{
    public static event Action<Transform> OnBoxOpen;

    [SerializeField] private GameObject catPrefab;

    private void OnMouseDown()
    {
        if (GameManager.Instance.IsPopupSceneLoaded) return;
        OnBoxOpen?.Invoke(transform.parent);
        Destroy(gameObject);
    }
}