using TMPro;
using UnityEngine;

public class CatalogGrid : MonoBehaviour
{
    [SerializeField] private GameObject _slotPrefab;

    [Header("Slot Popup")]
    [SerializeField] private GameObject _slotPopup;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _lore;
    [SerializeField] private TMP_Text _damage;
    [SerializeField] private TMP_Text _dps;
    [SerializeField] private TMP_Text _speed;
    [SerializeField] private TMP_Text _satiety;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private AdoptButton _adoptButton;

    private void Awake()
    {
        CatalogSlot.OnClick += InitCatalogSlot_OnClick;

        foreach (var cat in GameManager.Instance.Cats)
        {
            Instantiate(_slotPrefab, transform).GetComponent<CatalogSlot>().Init(cat);
        }
    }

    private void OnDestroy()
    {
        CatalogSlot.OnClick -= InitCatalogSlot_OnClick;
    }

    private void InitCatalogSlot_OnClick(CatSO cat)
    {
        _slotPopup.SetActive(true);
        //_renderer.sprite = cat.SpriteFront;
        _name.text = cat.Name;
        _level.text = cat.Level.ToString();
        _lore.text = cat.Lore.ToString();
        _damage.text = cat.Damage().ToString();
        _dps.text = cat.DPS().ToString();
        _speed.text = cat.Speed().ToString();
        _satiety.text = cat.Satiety().ToString();
        _price.text = GameManager.Instance.CatPrices[cat.Level - 1].ToString();

        _adoptButton.Init(cat);
    }
}