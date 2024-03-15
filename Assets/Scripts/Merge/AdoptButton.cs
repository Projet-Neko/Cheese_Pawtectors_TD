using System;
using TMPro;
using UnityEngine;

public class AdoptButton : MonoBehaviour
{
    public static event Action<int> OnAdoptButtonClick;

    [SerializeField] private CatSO _cat;

    [Header("HUD")]
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TMP_Text _catPrice;
    [SerializeField] private TMP_Text _catLevel;

    private int _cheapestCatIndex;

    private void Awake()
    {
        Mod_Economy.OnAdoptCheck += Mod_Economy_OnAdoptCheck;

        if (_renderer == null | _catLevel == null) UpdateCheapestCat();
    }

    private void OnDestroy()
    {
        Mod_Economy.OnAdoptCheck -= Mod_Economy_OnAdoptCheck;
    }

    public void Init(CatSO cat)
    {
        _cat = cat;
    }

    private void Mod_Economy_OnAdoptCheck(bool canAdopt, int catIndex)
    {
        // Update price if adopted
        if (!canAdopt) return;
        if (_cat == null) UpdateCheapestCat();

        // TODO -> show can't adopt modal popup if necessary
    }

    public void Buy()
    {
        int catLevel;

        if (_cat == null) catLevel = _cheapestCatIndex + 1;
        else catLevel = _cat.Level;

        OnAdoptButtonClick?.Invoke(catLevel);
    }

    private void UpdateCheapestCat()
    {
        _cheapestCatIndex = GameManager.Instance.GetCheapestCatIndex();
        _catPrice.text = GameManager.Instance.CatPrices[_cheapestCatIndex].ToString();
        _catLevel.text = (_cheapestCatIndex + 1).ToString();
        _renderer.sprite = GameManager.Instance.Cats[_cheapestCatIndex].SpriteFront;
    }
}