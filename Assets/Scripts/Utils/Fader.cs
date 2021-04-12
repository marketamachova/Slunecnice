using System.Collections;
using UnityEngine;

public class Fader : MonoBehaviour
{
    [SerializeField] private GameObject faderObjectPrefab;
    [SerializeField] private float fadeDuration;

    private Color _faderColor;
    private Material _fadeMaterial;
    private GameObject _faderObject;
    private Renderer _fadeRenderer;
    private float _alpha = 0f;

    private void Awake()
    {
    }

    public IEnumerator FadeCoroutine(bool fadeIn)
    {
        // yield return new WaitForSecondsRealtime(1);

        if (!fadeIn)
        {
            _faderObject = Instantiate(faderObjectPrefab, gameObject.transform);
            _fadeRenderer = _faderObject.GetComponent<Renderer>();
            _fadeMaterial = _fadeRenderer.material;
            _faderColor = _fadeMaterial.color;
            _alpha = 0f;
        }

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            if (fadeIn)
            {
                // _faderColor.a -= 0.1f;
                _alpha -= 0.03f;
            }
            else
            {
                // _faderColor = Color.cyan;
                // _faderColor.a += 0.1f;
                _alpha += 0.03f;
            }

            SetMaterialAlpha();
            yield return null;
        }

        if (fadeIn)
        {
            Destroy(_faderObject);
        }
    }

    public void FadeOut()
    {
        Debug.Log("fade out");
        StartCoroutine(FadeCoroutine(false));
    }

    public void FadeIn()
    {
        Debug.Log("fade in");
        StartCoroutine(FadeCoroutine(true));
    }
    
    private void SetMaterialAlpha()
    {
        Color color = _faderColor;
        color.a = _alpha;
        if (_fadeMaterial != null)
        {
            _fadeMaterial.color = color;
            // fadeMaterial.renderQueue = renderQueue;
            _fadeRenderer.material = _fadeMaterial;
        }
    }
}