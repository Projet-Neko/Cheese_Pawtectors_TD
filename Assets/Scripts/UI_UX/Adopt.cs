using UnityEngine;
using UnityEngine.UI;

public class Adopt : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefab;
    [SerializeField] private GridLayoutGroup _slots;

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