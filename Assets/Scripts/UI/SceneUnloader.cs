using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneUnloader : MonoBehaviour
{
    [SerializeField] AudioClip clip;
    private void Start()
    {
        Button button = GetComponent<Button>();

        button.onClick.AddListener(() => GameManager.Instance.SoundEffect(clip));
    }
    public void UnloadScene()
    {
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}