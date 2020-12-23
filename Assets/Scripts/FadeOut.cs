using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FadeOut : MonoBehaviour
{
    public GameObject obj;
    public GameObject bodyObj;
    public float fadeSpeed;
    private Color _color;
    private Renderer _renderer;
    private bool _quitFade;

    void Start()
    {
        _renderer = bodyObj.GetComponent<Renderer>();
        _color = _renderer.material.color;
        _quitFade = false;
    }

    void Update()
    {
        if (!_quitFade)
        {
            if (_color.a <= 0)
            {
                _quitFade = true;
                Destroy();
                enabled = false;
                return;
            }
            
            var alpha = _color.a - (fadeSpeed * Time.deltaTime);

            _color = new Color(_color.r, _color.g, _color.b, alpha);
            bodyObj.GetComponent<Renderer>().material.color = _color;
        }
    }

    public void Destroy()
    {
        Destroy(obj);
        _quitFade = true;
    }
}
