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
        _quizAnswers = new int[4];
        SetUI();
    }

    public void NextQuestion(int buttonIndex)
    {
        int clan = (int)_questionsData[_questionIndex].Answers.ElementAt(buttonIndex).Key;
        //Debug.Log($"Clicked on {(Clan)clan} answer");

        _quizAnswers[clan]++;

        for (int i = 0; i < _quizAnswers.Length; i++)
        {
            //Debug.Log($"{(Clan)i} - {_quizAnswers[i]} answers");
        }

        _questionIndex++;

        if (_questionIndex == _questionsData.Length)
        {
            clan = 0;
            Debug.Log(_quizAnswers.Length);

            for (int i = 0; i < _quizAnswers.Length; i++)
            {
                //Debug.Log($"{(Clan)i} - {_quizAnswers[i]} answers");
                if (clan > _quizAnswers[i]) clan = _quizAnswers[i];
            }

            GameManager.Instance.SetUserClan(clan);
            _resultPanel.SetActive(true);
            _resultClanName.text = ((Clan)clan).ToString();
            _resultClanDescription.text = GameManager.Instance.Clans[clan].Description;
            _resultGem = GameManager.Instance.Clans[clan].Gem;
        }
        else SetUI();
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