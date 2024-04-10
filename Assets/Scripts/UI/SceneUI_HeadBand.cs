using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI_HeadBand : MonoBehaviour
{
    [SerializeField] private TMP_Text _username;
    [SerializeField] private TMP_Text _waveNumber;
    //[SerializeField] private TMP_Text _score;
    [SerializeField] private TMP_Text _treatsNumber;
    [SerializeField] private TMP_Text _treatsPerSecond;
    [SerializeField] private TMP_Text _goldNumber;
    [SerializeField] private Slider _enemiesNumber;
    [SerializeField] private Image _profilePicture;

    // TODO -> optimize with event instead of update

    private void Update()
    {
        _username.text = GameManager.Instance.Username;
        _waveNumber.text = "WAVE " + GameManager.Instance.Data.WaveNumber.ToString();
        //_score.text = GameManager.Instance.Data.Score.ToString();
        _treatsNumber.text = CurrencyFormat.Get(GameManager.Instance.Data.Currencies[(int)Currency.Treats].Amount);
        _treatsPerSecond.text = $"(+{GameManager.Instance.TreatPerSecond()}/sec)";
        _goldNumber.text = CurrencyFormat.Get(GameManager.Instance.Data.Currencies[(int)Currency.Meowcoin].Amount);
        _enemiesNumber.value = GameManager.Instance.KilledEnemiesNumber;
        _enemiesNumber.maxValue = GameManager.Instance.MaxEnemyNumber;
        //Debug.Log($"{GameManager.Instance.EnemyNumber}/{GameManager.Instance.MaxEnemyNumber}");
    }
}