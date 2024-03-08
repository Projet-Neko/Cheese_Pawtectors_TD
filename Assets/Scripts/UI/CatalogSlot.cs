using TMPro;
using UnityEngine;

public class CatalogSlot : MonoBehaviour
{
    [SerializeField] private TMP_Text _name;
    [SerializeField] private TMP_Text _level;
    [SerializeField] private TMP_Text _price;
    [SerializeField] private SpriteRenderer _renderer;

    public void Init(CatSO catSO)
    {
        _name.text = catSO.Name;
        _level.text = catSO.Level.ToString();
        _price.text = GameManager.Instance.CatPrices[catSO.Level - 1].ToString();
        _renderer.sprite = catSO.SpriteFront;
    }
}
