using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private GameObject faderObjectPrefab;
    [SerializeField] private float fadeDuration;
    [SerializeField] private GameObject canvas;

    private Color _faderColor;
    private Material _fadeMaterial;
    private GameObject _faderObject;
    private Renderer _fadeRenderer;
    private float _alpha = 0f;

    public IEnumerator FadeCoroutine(bool fadeIn)
    {
        if (!fadeIn)
        {
            _faderObject = Instantiate(faderObjectPrefab, gameObject.transform);
            _fadeRenderer = _faderObject.GetComponent<Renderer>();
            _fadeMaterial = _fadeRenderer.material;
            _faderColor = _fadeMaterial.color;
            _alpha = 0f;
        }

        canvas.SetActive(true);

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            if (fadeIn)
            {
                _alpha -= 0.03f;
            }
            else
            {
                _alpha += 0.03f;
            }

            SetMaterialAlpha();
            yield return null;
        }

        if (fadeIn)
        {
            Destroy(_faderObject);
        }
        
        canvas.SetActive(false);
    }

    public void FadeOut()
    {
        StartCoroutine(FadeCoroutine(false));
    }

    public void FadeIn()
    {
        StartCoroutine(FadeCoroutine(true));
    }
    
    private void SetMaterialAlpha()
    {
        Color color = _faderColor;
        color.a = _alpha;
        if (_fadeMaterial != null)
        {
            _fadeMaterial.color = color;
            _fadeRenderer.material = _fadeMaterial;
        }
    }
}