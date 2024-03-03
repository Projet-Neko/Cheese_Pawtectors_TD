using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Adopt : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private GridLayoutGroup _slots;
    [SerializeField] private GameObject _catPrefab;

    [Header("HUD")]
    [SerializeField] TMP_Text _catPrice;
    [SerializeField] TMP_Text _catLevel;

    private int _cheapestCatIndex;

    private void Awake()
    {
        UpdateCheapestCat();
    }

    // Instantiate the cat bought in empty slots when a button "Adopt" is clicked.
    public void OnButtonClick()
    {
        Transform freeSlot = null;

        foreach (Transform slot in _slots.transform)
        {
            if (slot.childCount == 0)
            {
                freeSlot = slot;
                break;
            }
        }

        if (freeSlot == null) return;

        if (GameManager.Instance.CanAdopt(_cheapestCatIndex))
        {
            GameObject go = Instantiate(_catPrefab, freeSlot);
            go.transform.localScale = new Vector3(10, 10, 10);

            Cat cat = go.GetComponent<Cat>();
            cat.Init(_cheapestCatIndex + 1);
            cat.SetStorageMode(true); // Permet de cacher le HUD

            UpdateCheapestCat();
        }
    }

    private void UpdateCheapestCat()
    {
        _cheapestCatIndex = GameManager.Instance.GetCheapestCatIndex();
        _catPrice.text = GameManager.Instance.CatPrices[_cheapestCatIndex].ToString();
    }
}