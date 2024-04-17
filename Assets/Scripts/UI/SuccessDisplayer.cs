using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SuccessDisplayer : MonoBehaviour
{
    [SerializeField] private GameObject _successBanner;

    private bool _initialisation = false;

    void Awake()
    {
        Success_Manager.DisplaySuccessEvent += DisplaySuccess;
    }

    private void OnDestroy()
    {
        Success_Manager.DisplaySuccessEvent -= DisplaySuccess;
    }

    private void DisplaySuccess(List<SuccessSO> SuccessList)
    {
        foreach (var success in SuccessList)
        {
            GameObject newSuccess = Instantiate(_successBanner, transform);
            TextMeshProUGUI text = newSuccess.GetComponentsInChildren<TextMeshProUGUI>()[0];
            TextMeshProUGUI sliderText = newSuccess.GetComponentsInChildren<TextMeshProUGUI>()[1];
            Slider slider = newSuccess.GetComponentInChildren<Slider>();
            newSuccess.name = success.name;
            text.text = success._successame;
            slider.value = success._progression;
            slider.maxValue = success._step;
            sliderText.text = success._progression + "/" + success._step;
        }
    }
}
