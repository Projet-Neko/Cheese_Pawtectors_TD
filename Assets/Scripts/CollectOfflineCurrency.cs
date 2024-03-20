using System;
using UnityEngine;

public class CollectOfflineCurrency : MonoBehaviour
{
    public static event Action OnCollect;

    public void OnClick()
    {
        OnCollect?.Invoke();
    }
}