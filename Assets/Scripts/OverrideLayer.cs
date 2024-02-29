using UnityEngine;

public class OverrideLayer : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().overrideSorting = true;
    }
}