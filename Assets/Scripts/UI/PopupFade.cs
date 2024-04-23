using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

// HOW TO USE :
// On Animation component : add FadeIn clip as "Animation" + add FadeOut clip in "Animations"
// Don't forget to serialize fields

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Animation))]
public class PopupFade : MonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private CanvasGroup _canvasGroup;
    [SerializeField] private Animation _animation;

    [Header("Animation Clips")]
    [SerializeField] private AnimationClip _fadeIn;
    [SerializeField] private AnimationClip _fadeOut;

    [Header("Options")]
    [SerializeField] private int _duration = 5;

    private void Awake()
    {
        _canvasGroup.alpha = 0;
        StartCoroutine(Fade());
    }

    private IEnumerator Fade()
    {
        yield return new WaitForSeconds(_fadeIn.length + _duration);
        _animation.Play(_fadeOut.name);
        yield return new WaitForSeconds(_fadeOut.length);
        SceneManager.UnloadSceneAsync(gameObject.scene);
    }
}