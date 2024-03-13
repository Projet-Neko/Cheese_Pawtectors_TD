using UnityEngine;

public class SetPopup : MonoBehaviour
{
    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        //canvas.sortingOrder = 10;
    }
}