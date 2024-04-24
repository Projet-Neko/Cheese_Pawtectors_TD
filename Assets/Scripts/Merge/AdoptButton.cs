using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdoptButton : MonoBehaviour
{
    public static event Action<int> OnAdoptButtonClick;

    [SerializeField] private CatSO _cat;

    [Header("HUD")]
    [SerializeField] private Image _catSprite;
    [SerializeField] private TMP_Text _catPrice;
    [SerializeField] private TMP_Text _catLevel;

    private int _cheapestCatIndex;

    private void Awake()
    {
        Mod_Economy.OnAdoptCheck += Mod_Economy_OnAdoptCheck;

        if (_catLevel != null) UpdateCheapestCat();
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
        if (!canAdopt) return;
        if (_cat == null) UpdateCheapestCat();
    }

    public void Buy()
    {
        int catLevel;

        if (_cat == null) catLevel = _cheapestCatIndex + 1;
        else catLevel = _cat.Level;

        if (!GameManager.Instance.Data.CatsUnlocked[catLevel - 1])
        {
            GameManager.Instance.LogError("Cat is not unlock yet !");
            return;
        }

        OnAdoptButtonClick?.Invoke(catLevel);
    }

    private void UpdateCheapestCat()
    {
        _cheapestCatIndex = GameManager.Instance.GetCheapestCatIndex();
        _catPrice.text = GameManager.Instance.CatPrices[_cheapestCatIndex].ToString();
        _catLevel.text = (_cheapestCatIndex + 1).ToString();
        _catSprite.sprite = GameManager.Instance.Cats[_cheapestCatIndex].Sprites[4];
    }
}