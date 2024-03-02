using UnityEngine;
using UnityEngine.UI;

public class LoadingSriptUI : MonoBehaviour
{
    [SerializeField] Image _circle;

    [SerializeField][Range(0, 1)] float _progress = 0f;
    void Update()
    {
        _circle.fillAmount = _progress;
    }
}