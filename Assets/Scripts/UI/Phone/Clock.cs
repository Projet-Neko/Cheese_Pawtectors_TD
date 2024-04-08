using TMPro;
using UnityEngine;

public class Clock : MonoBehaviour
{
    private TMP_Text clockText;

    void Start()
    {
        clockText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (clockText != null)
        {
            // Obtient l'heure actuelle
            string time = System.DateTime.Now.ToString("HH:mm");

            clockText.text = time;
        }
        else
        {
            Debug.LogError("Composant TextMeshPro non trouvé sur l'objet attaché.");
        }
    }
}
