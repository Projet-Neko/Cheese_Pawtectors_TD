using UnityEngine;
using UnityEngine.UI;

public class Adopt : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private GridLayoutGroup _slots;
    private int _price; // TODO : get the real price of the cat we want to buy

    public bool CanAdoptCat()
    {
        int meat = GameManager.Instance.Meat;
        return true;
    }

    public void OnButtonClick()
    {
        // Parcoure tous les enfants (slots) de l'objet Slots
        foreach (Transform slot in _slots.transform)
        {
            if (slot.childCount == 0)
            {
                GameObject go = Instantiate(_catPrefab, slot);
                go.transform.localScale = new Vector3(100, 100, 100);
                return;
            }
        }
    }
}