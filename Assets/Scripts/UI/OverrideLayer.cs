using UnityEngine;

// � placer sur les canvas pour utiliser le Sorting Order

public class OverrideLayer : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<Canvas>().overrideSorting = true;
    }
}