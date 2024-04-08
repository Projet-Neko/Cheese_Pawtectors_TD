using System.Collections;
using TMPro;
using UnityEngine;

public class ShowLoadingTips : MonoBehaviour
{
    [SerializeField] private TMP_Text _loadingTips;
    [SerializeField] private TipsSO _tips;

    private void Awake()
    {
        StartCoroutine(ShowTips());
    }

    private IEnumerator ShowTips()
    {
        while (true)
        {
            _loadingTips.text = _tips.Tips[Random.Range(0, _tips.Tips.Length)];
            yield return new WaitForSeconds(Random.Range(2, 4));
        }
    }
}