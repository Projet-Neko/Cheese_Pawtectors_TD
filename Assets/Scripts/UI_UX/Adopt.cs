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
        SetCheapestCatIndex();
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
        int catIndex = SetCheapestCatIndex();
        if (GameManager.Instance.CanAdopt(catIndex))
        {
            GameObject go = Instantiate(_catPrefab, freeSlot);
            Cat cat = go.GetComponent<Cat>();
            cat.Init(catIndex);
            go.transform.localScale = new Vector3(10, 10, 10);
            go.GetComponent<Cat>().SetStorageMode(true); // Permet de cacher le HUD
            SetCheapestCatIndex();
        }
    }
    public int SetCheapestCatIndex()
    {
        int i = GameManager.Instance.GetCheapestCatIndex();
        _catPrice.text = GameManager.Instance.CatPrices[i].ToString();
        return i;
    }
}