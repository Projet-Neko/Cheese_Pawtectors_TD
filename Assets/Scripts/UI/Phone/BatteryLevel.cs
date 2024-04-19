using TMPro;
using UnityEngine;

public class BatteryLevel : MonoBehaviour
{
    private TMP_Text batteryText;

    void Start()
    {
        batteryText = GetComponent<TMP_Text>();
    }

    void Update()
    {
        if (batteryText != null)
        {
            // Obtient le niveau de la batterie en pourcentage (0-1)
            float batteryLevel = SystemInfo.batteryLevel;

            // Convertit en pourcentage (0-100)
            int batteryPercent = Mathf.FloorToInt(batteryLevel * 100);

            batteryText.text = batteryPercent.ToString() + "%";
        }
        else
        {
            Debug.LogError("Composant TextMeshPro non trouvé sur l'objet attaché.");
        }
    }
}
