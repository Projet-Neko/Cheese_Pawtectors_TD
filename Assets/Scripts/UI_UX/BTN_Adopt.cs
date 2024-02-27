using UnityEngine;
using UnityEngine.UI;

public class BTN_Adopt : MonoBehaviour
{
    [SerializeField] private GameObject _catPrefabSF;
    [SerializeField] private GridLayoutGroup _slotsSF;

    public void OnButtonClick()
    {
        // Parcoure tous les enfants (slots) de l'objet Slots
        foreach (Transform slot in _slotsSF.transform)
        {
            // Vérifie s'il n'y a pas déjà un chat dans le slot
            if (slot.childCount == 0)
            {
                // S'il n'y a pas de chat, instancie un nouveau chat dans ce slot
                Instantiate(_catPrefabSF, slot);
                return; // Sort de la fonction après avoir placé le chat
            }
        }
    }
}
