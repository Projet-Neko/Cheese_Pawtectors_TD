using UnityEngine;
using UnityEngine.UI;

public class Adopt : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private GridLayoutGroup _slots;
    private int _catPrice = 1; // TODO : get the real price of the cat we want to buy
    private bool _canAdoptCat = false;

    // Deducts the cat's price from the player's meat resource if they can adopt a cat.
    public void DeductCatPrice()
    {
        // Debug.Log($" You paid : {_catPrice}, to adopt your new cat");
        GameManager.Instance.RemoveMeat(_catPrice);
    }

    // Verify if we have enough meat currency to adopt a cat
    public void CheckAndSetCanAdoptCat()
    {
        int meat = GameManager.Instance.Meat;
        if (meat >= _catPrice)
        {
            _canAdoptCat = true;
            Debug.Log($" Congratulation, you've adopted a cat !");
        }
        else
        {
            _canAdoptCat = false;
            Debug.Log($" You can't adopt this cat not enough money!");
        }
    }


    // Instantiate the cat bought in empty slots when a button "Adopt" is clicked.
    public void OnButtonClick()
    {
        CheckAndSetCanAdoptCat();
        if (_canAdoptCat != true)
        {
            // TODO : Notify player that he doesn't have enough meat to buy this cat
            return;
        }
        // Parcoure tous les enfants (slots) de l'objet Slots
        foreach (Transform slot in _slots.transform)
        {
            if (slot.childCount == 0)
            {
                DeductCatPrice();
                GameObject go = Instantiate(_catPrefab, slot);
                go.transform.localScale = new Vector3(100, 100, 100);
                go.GetComponent<Cat>().SetStorageMode(true);
                return;
            }
        }
    }
}