using UnityEngine;

public class QuitButton : MonoBehaviour
{
    [SerializeField] private GameObject _closePopupBackground;

    // Called when the player clicks the "Quit" button in Build scene
    public void CheckPathHouse()
    {
        House.Instance.BuildPath();
        bool validPath = House.Instance.ValidatePath();

        if (validPath)
            _closePopupBackground.SetActive(true);
    }
}
