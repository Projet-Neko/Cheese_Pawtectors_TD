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
        Mod_Economy.OnAdoptCheck += Mod_Economy_OnAdoptCheck;
    }

    private void OnDestroy()
    {
        CatalogPopup.OnToggle -= InitCatalog_OnToggle;
        Mod_Economy.OnAdoptCheck -= Mod_Economy_OnAdoptCheck;
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

        GetComponentInChildren<AdoptButton>().Init(catSO);
    }

    private void OnMouseUp()
    {
        if (_isPopupOpened) return;
        OnClick?.Invoke(_data);
    }

    private void Mod_Economy_OnAdoptCheck(bool canAdopt, int catLevel)
    {
        if (!canAdopt | catLevel != _data.Level) return;
        _price.text = GameManager.Instance.CatPrices[_data.Level - 1].ToString();
    }
}