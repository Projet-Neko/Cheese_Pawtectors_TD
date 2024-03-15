using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugEmilie : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
    }

    private void OnMouseDown()
    {
        Debug.Log("Debug - On Mouse Down");
    }
}