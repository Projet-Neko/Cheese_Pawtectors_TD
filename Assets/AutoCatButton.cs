using UnityEngine;
using UnityEngine.UI;

public class AutoCatButton : MonoBehaviour
{
    [SerializeField] AudioClip clip;

    private void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(() => GameManager.Instance.SoundEffect(clip));
    }
}
