using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public float Speed { get; set; } = 2;
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
        SetProgressBarPlaying(false);
    }

    public void SetProgressBarValue(float value)
    {
        time = value;
        UpdateProgressBar();
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
        switch (speed)
        {
            case 1:
                totalSeconds = 780;
                break;
            case 2:
                totalSeconds = 390;
                break;
            case 3:
                totalSeconds = 270;
                break;
            case 4:
                totalSeconds = 200;
                break;
            default:
                totalSeconds = 160;
                break;
        }
    }
    
}
