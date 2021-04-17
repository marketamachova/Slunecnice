using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIControllerMobile : BaseUIController
    {
        [Header("Runtime")] [SerializeField] public bool portraitOriented;
        [SerializeField] private bool controlsVisible;


        [Header("Mobile UI Elements")] [SerializeField]
        private List<GameObject> cameraViews;

        [SerializeField] private GameObject controls;
        [SerializeField] private Timer timer;
        [SerializeField] private ProgressBar progressBar;

        [Header("Special Elements")]
        [SerializeField] private Selectable playButton;
        [SerializeField] private Selectable maximizeButton;
        [SerializeField] private MobileController mobileController;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private TextMeshProUGUI sliderText;

        private string _panelOnTopOfStack;

        private void Awake()
        {
            networkManager.OnClientDisconnectAction += DisplayError;
        }

        public void EnableCameraView(string cameraViewName)
        {
            cameraViews.ForEach(panel => panel.SetActive(panel.name == cameraViewName));
        }

        public void OnChangeScreenOrientation()
        {
            Screen.orientation = portraitOriented ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait;
            portraitOriented = !portraitOriented;
            maximizeButton.SetSelected(!portraitOriented);
            Debug.Log(portraitOriented);
        }

        public void ToggleControlsVisible()
        {
            controlsVisible = !controlsVisible;
            controls.SetActive(controlsVisible);
        }

        private IEnumerator HideControls()
        {
            while (controlsVisible)
            {
                yield return new WaitForSecondsRealtime(5);
                controls.SetActive(false);
            }
        }

        public void OnPlayPressed(bool playing)
        {
            playButton.SetSelected(playing);
            timer.SetTimerPlaying(playing);
            progressBar.SetProgressBarPlaying(playing);
        }


        //general controller
        public void OnSceneChosen(string chosenScene)
        {
            mobileController.OnSceneSelected(chosenScene);
            timer.ResetTimer(); //nevim
        }

        public void AddPanelToStack(string panelName)
        {
            _panelOnTopOfStack = panelName;
            Enable(panelName, true);
            Enable("BackButton", true);
        }

        public void RemovePanelFromStack()
        {
            Enable(_panelOnTopOfStack, false);
            Enable("BackButton", false);
        }


        public void SetPlayButtonSelected(bool playing) //TODO
        {
            var playButtonImage = playButton.gameObject.GetComponent<Image>();
            if (playing)
            {
                playButtonImage.sprite = Resources.Load<Sprite>("UI/PlayButtonActive");
            }
            else
            {
                playButtonImage.sprite = Resources.Load<Sprite>("UI/PlayButton");
            }
        }

        public void UpdateTimer(bool playing, float time)
        {
            timer.SetTime(time);
            timer.SetTimerPlaying(playing);
            progressBar.SetProgressBarPlaying(playing);
            progressBar.SetProgressBarValue(time);
        }

        public void OnSpeedSliderUpdate()
        {
            sliderText.text = speedSlider.value.ToString();
            mobileController.SetSpeed((int) speedSlider.value);
        }
    }
}