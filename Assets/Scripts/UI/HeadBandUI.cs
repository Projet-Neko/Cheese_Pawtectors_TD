using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HeadBandUI : MonoBehaviour
{
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_Text _waveNumber;
    [SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _meatNumber;
    [SerializeField] private TMP_Text _meatPerSecond;
    [SerializeField] private TMP_Text _mewstonesNumber;
    [SerializeField] private Slider _enemiesNumber;
    [SerializeField] private Image _profilePicture;

    // TODO -> optimize with event instead of update

    private void Update()
    {
        _username.text = GameManager.Instance.Username;
        _waveNumber.text = "WAVE " + GameManager.Instance.Data.WaveNumber.ToString();
        _score.text = GameManager.Instance.Data.Score.ToString();
        _meatNumber.text = GameManager.Instance.Currencies[Currency.Meat].ToString(); // UC
        _meatPerSecond.text = $"(+{GameManager.Instance.MeatPerSecond()}/sec)";
        _mewstonesNumber.text = GameManager.Instance.Currencies[Currency.Meowstone].ToString(); // UC
        _enemiesNumber.value = GameManager.Instance.EnemyNumber;
        _enemiesNumber.maxValue = GameManager.Instance.MaxEnemyNumber;
        //Debug.Log($"{GameManager.Instance.EnemyNumber}/{GameManager.Instance.MaxEnemyNumber}");
    }
}