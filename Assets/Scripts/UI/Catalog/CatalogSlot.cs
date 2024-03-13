using System;
using TMPro;
using UnityEngine;

public class CatalogSlot : MonoBehaviour
{
    public static event Action<CatSO> OnClick;

    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private SpriteRenderer _renderer;

    private CatSO _data;
    private bool _isPopupOpened = false;

    private void Awake()
    {
        CatalogPopup.OnToggle += InitCatalog_OnToggle;
    }

    private void OnDestroy()
    {
        CatalogPopup.OnToggle -= InitCatalog_OnToggle;
    }

    private void InitCatalog_OnToggle(bool isOpened)
    {
        _isPopupOpened = isOpened;
    }

    public void Init(CatSO catSO)
    {
        _data = catSO;
        _name.text = catSO.Name;
        _level.text = catSO.Level.ToString();
        _price.text = GameManager.Instance.CatPrices[catSO.Level - 1].ToString();
        _renderer.sprite = catSO.SpriteFront;
    }

    private void OnMouseUp()
    {
        Debug.Log("on mouse up");
        if (_isPopupOpened) return;
        Debug.Log("popup is not opened");
        OnClick?.Invoke(_data);
    }
}