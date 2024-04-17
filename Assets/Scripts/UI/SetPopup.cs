using UnityEngine;
using UnityEngine.UI;

public class SetPopup : MonoBehaviour
{
    public ScrollRect scrollRect;
    private string scrollPositionKey = "ScrollPosition";

    private void Awake()
    {
        Canvas canvas = GetComponent<Canvas>();
        canvas.worldCamera = Camera.main;
        //canvas.sortingOrder = 10;

        scrollRect = GetComponentInChildren<ScrollRect>(); // Trouver la ScrollRect dans les enfants du canvas
        if (scrollRect == null)
        {
            Debug.LogError("ScrollRect not found in children.");
            return;
        }

        // Load saved ScrollRect position on popup activation
        Vector2 savedPosition = new Vector2(PlayerPrefs.GetFloat(scrollPositionKey + "X", 0f),
                                            PlayerPrefs.GetFloat(scrollPositionKey + "Y", 0f));
        scrollRect.normalizedPosition = savedPosition;
    }

    private void OnDisable()
    {
        if (scrollRect == null)
        {
            // Save the current position of the ScrollRect when the popup is deactivated
            PlayerPrefs.SetFloat(scrollPositionKey + "X", scrollRect.normalizedPosition.x);
            PlayerPrefs.SetFloat(scrollPositionKey + "Y", scrollRect.normalizedPosition.y);
            PlayerPrefs.Save(); // Save changes in PlayerPrefs
        }
    }
}