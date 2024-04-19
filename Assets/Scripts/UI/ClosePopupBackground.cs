using UnityEngine;

public class ClosePopupBackground : MonoBehaviour
{
    [SerializeField] private GameObject _popupToClose;

    public void ClosePopup()
    {
        _popupToClose.SetActive(false);
    }
}