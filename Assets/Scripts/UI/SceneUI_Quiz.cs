using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneUI_Quiz : MonoBehaviour
{
    [Header("Quiz")]
    [SerializeField] private TMP_Text _questionNumber;
    [SerializeField] private TMP_Text _question;
    [SerializeField] private TMP_Text[] _answers;

    [Header("Result Panel")]
    [SerializeField] private GameObject _resultPanel;
    [SerializeField] private Image _resultGem;
    [SerializeField] private TMP_Text _resultClanName;
    [SerializeField] private TMP_Text _resultClanDescription;

    private ClanQuestionSO[] _questionsData;
    private int[] _quizAnswers;
    private int _questionIndex = 0;

    private void Awake()
    {
        _questionsData = Resources.LoadAll<ClanQuestionSO>("SO/ClanQuestions");
        _quizAnswers = new int[_questionsData.Length];
        SetUI();
    }

    public void NextQuestion(int buttonIndex)
    {
        int clan = (int)_questionsData[_questionIndex].Answers.ElementAt(buttonIndex).Key;

        if (_questionIndex < _questionsData.Length - 1)
        {
            _quizAnswers[clan]++;
            _questionIndex++;
            SetUI();
        }
        else
        {
            GameManager.Instance.GetChoosenClan(_quizAnswers);
            _resultPanel.SetActive(true);
            _resultClanName.text = ((Clan)clan).ToString();
            _resultClanDescription.text = GameManager.Instance.Clans[clan].Description;
            _resultGem = GameManager.Instance.Clans[clan].Gem;
        }
    }

    private void SetUI()
    {
        _questionNumber.text = (_questionIndex + 1).ToString();
        _question.text = _questionsData[_questionIndex].Question.ToString();

        int i = 0;

        foreach (var answer in _questionsData[_questionIndex].Answers)
        {
            _answers[i].text = answer.Value;
            i++;
        }
    }
}