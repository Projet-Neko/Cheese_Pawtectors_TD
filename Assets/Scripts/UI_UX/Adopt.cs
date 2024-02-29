using UnityEngine;
using UnityEngine.UI;

public class Adopt : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private GridLayoutGroup _slots;

    private int _catPrice = 1; // TODO : get the real price of the cat we want to buy

    // Instantiate the cat bought in empty slots when a button "Adopt" is clicked.
    public void OnButtonClick()
    {
        if (GameManager.Instance.Meat < _catPrice)
        {
            // TODO : Notify player that he doesn't have enough meat to buy this cat
            Debug.Log($" You can't adopt this cat not enough money!");
            return;
        }

        // Parcoure tous les enfants (slots) de l'objet Slots
        foreach (Transform slot in _slots.transform)
        {
            if (slot.childCount == 0)
            {
                GameManager.Instance.RemoveMeat(_catPrice);
                GameObject go = Instantiate(_catPrefab, slot);
                go.transform.localScale = new Vector3(10, 10, 10);
                go.GetComponent<Cat>().SetStorageMode(true); // Permet de cacher le HUD
                return;
            }
        }
    }
}