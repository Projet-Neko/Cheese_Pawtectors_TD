using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TabsManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _Tabs;
    [SerializeField] private Image[] _TabsButtons;
    [SerializeField] private Color _InactiveTabBg, _ActiveTabBg;
    [SerializeField] private Vector2 _InactiveTabButtonSize, _ActiveTabButtonSize;



    public void SwitchToTab(int TabID)
    {
        foreach (GameObject go in _Tabs) { go.SetActive(false); }
        _Tabs[TabID].SetActive(true);

        foreach (Image im in _TabsButtons)
        {
            im.color = _InactiveTabBg;
            im.rectTransform.sizeDelta = _InactiveTabButtonSize;
        }

        _TabsButtons[TabID].color = _ActiveTabBg;
        _TabsButtons[TabID].rectTransform.sizeDelta = _ActiveTabButtonSize;

    }
}