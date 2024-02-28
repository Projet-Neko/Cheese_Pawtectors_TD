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
            // Vérifie s'il n'y a pas déjà un chat dans le slot
            if (slot.childCount == 0)
            {
                // S'il n'y a pas de chat, instancie un nouveau chat dans ce slot
                GameObject go = Instantiate(_catPrefab, slot);
                go.transform.localScale = new Vector3(100, 100, 100);
                return; // Sort de la fonction après avoir placé le chat
            }
        }
    }
}