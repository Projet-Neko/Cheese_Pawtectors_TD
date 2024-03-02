using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Adopt : MonoBehaviour
{
    [SerializeField] TMP_Text _catPrice;
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private GridLayoutGroup _slots;

    private void Awake()
    {
        _catPrice.text = GameManager.Instance.CatPrices[0].ToString();
    }

    // Instantiate the cat bought in empty slots when a button "Adopt" is clicked.
    public void OnButtonClick()
    {
        Transform freeSlot = null;

        // Parcoure tous les enfants (slots) de l'objet Slots
        foreach (Transform slot in _slots.transform)
        {
            if (slot.childCount == 0)
            {
                freeSlot = slot;
                break;
            }
        }

        if (freeSlot == null) return;

        if (GameManager.Instance.CanAdopt(1))
        {
            GameObject go = Instantiate(_catPrefab, freeSlot);
            go.transform.localScale = new Vector3(10, 10, 10);
            go.GetComponent<Cat>().SetStorageMode(true); // Permet de cacher le HUD
            _catPrice.text = GameManager.Instance.CatPrices[0].ToString();
        }
    }
}