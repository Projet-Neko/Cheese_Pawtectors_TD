using UnityEngine;
using UnityEngine.UI;

public class PawClick : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float _timer = 1.0f;
    [SerializeField] private float _maxTimer = 1.0f;

    [Header("UI")]
    [SerializeField] private Image _pawUI = null;

    [Header("Key Code")]
    [SerializeField] private KeyCode _key = KeyCode.Mouse0;

    private bool _update = false;

    private void Update()
    {
        if (Input.GetKey(_key)) {
            _update = false;
            _timer -= Time.deltaTime;
            _pawUI.enabled = true;
            _pawUI.fillAmount = _timer;

            if (_timer <= 0) {
                _timer = _maxTimer;
                _pawUI.fillAmount = _maxTimer;
                _pawUI.enabled = false;
            }
        }
        else {
            if (_update) {
                _timer += Time.deltaTime;
                _pawUI.fillAmount = _timer;

                if (_timer >= _maxTimer) {
                    _timer = _maxTimer;
                    _pawUI.fillAmount = _maxTimer;
                    _pawUI.enabled = false;
                    _update = false;
                }
            }
        }

        if (Input.GetKeyUp(_key)) {
            _update = true;
        }
    }
}
