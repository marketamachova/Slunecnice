using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private GameObject fadeIn;
    [SerializeField] private float fadeDuration;

    private CanvasGroup _canvasGroup;

    private void Awake()
    {
        _canvasGroup = fadeIn.GetComponent<CanvasGroup>();
    }

    public IEnumerator FadeCoroutine()
    {
        yield return new WaitForSecondsRealtime(2);

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            _canvasGroup.alpha -= 0.05f;
            yield return null;
        }

        Destroy(fadeIn);
    }
}