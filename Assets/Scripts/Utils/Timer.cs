using TMPro;
using UnityEngine;

namespace Utils
{
    public class Timer : MonoBehaviour
    {
        private bool _playing = false;
        [SerializeField] private TextMeshProUGUI timerText;
        private float _totalTime = 0f;


        void Update()
        {
            if (_playing)
            {
                _totalTime += Time.deltaTime;
                UpdateTextTimer();
            }
        }

        private void UpdateTextTimer()
        {
            float minutes = Mathf.FloorToInt((_totalTime + 1) / 60);
            float seconds = Mathf.FloorToInt((_totalTime + 1) % 60);


            if (seconds < 10)
            {
                timerText.text = $"{minutes}:0{seconds}";
            }
            else
            {
                timerText.text = $"{minutes}:{seconds}";
            }
        }

        public void ResetTimer()
        {
            _totalTime = 0f;
            UpdateTextTimer();
            SetTimerPlaying(false);
        }

        public void SetTimerPlaying(bool playing)
        {
            _playing = playing;
        }

        public void SetTime(float time)
        {
            this._totalTime = time;
            UpdateTextTimer();
            SetTimerPlaying(true);
        }
    }
}