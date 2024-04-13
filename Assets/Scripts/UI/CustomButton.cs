using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour
{
    [SerializeField] AudioClip _clip;
    private void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(() => GameManager.Instance.SoundEffect(_clip));
    }
}
