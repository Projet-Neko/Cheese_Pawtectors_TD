using UnityEngine;

public class QuitButtonYes : MonoBehaviour
{
    // Called when the player clicks the "Quit" button in Build scene and clicks "Yes" in the popup
    public void DestroyInvalidRoom()
    {
        House.Instance.DestroyInvalidRoom();
    }
}
