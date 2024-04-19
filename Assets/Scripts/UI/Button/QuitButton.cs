using UnityEngine;

public class QuitButton : MonoBehaviour
{
    [SerializeField] private GameObject _closePopupBackground;
    [SerializeField] private House _house;

    // Called when the player clicks the "Quit" button in Build scene
    public void CheckPathHouse()
    {
        bool validPath = _house.ValidatePath();

        if (validPath)
            _closePopupBackground.SetActive(true);
    }
}
