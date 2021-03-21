using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    [SerializeField] private float totalSeconds;
    [SerializeField] private Slider progressBar;
    [SerializeField] private float time;
    private bool _playing = false;

    void Update()
    {
        if (_playing)
        {
            time += Time.deltaTime;
            UpdateProgressBar();
        }
    }

    private void UpdateProgressBar()
    {
        progressBar.value = time / totalSeconds;
    }

    public void SetProgressBarPlaying(bool playing)
    {
        _playing = playing;
    }

    public void ResetProgressBar()
    {
        time = 0f;
        UpdateProgressBar();
    }
    
}
