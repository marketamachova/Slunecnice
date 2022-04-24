using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utils;

namespace UI
{
    public enum PlayMode
    {
        PlayerCamera,
        TopCamera,
        Multiview,
        None
    }

    public class UIControllerMobile : BaseUIController
    {
        [Header("Runtime")] [SerializeField] public bool portraitOriented;
        [SerializeField] private bool controlsVisible;
        [SerializeField] private PlayMode currentPlayMode;

        [Header("Mobile UI Elements")] 
        [SerializeField] private List<GameObject> cameraViews;
        [SerializeField] private GameObject controls;
        [SerializeField] private GameObject controlButtons;
        [SerializeField] private Timer timer;
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private Button[] sceneButtons;
        [SerializeField] private Selectable playButton;
        [SerializeField] private Selectable maximizeButton;
        [SerializeField] private MobileController mobileController;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private TextMeshProUGUI sliderText;
        [SerializeField] private Button endButton;
        [SerializeField] private GameObject splashScreen;

        private string _panelOnTopOfStack;
        private PlayerCameraDisplayStrategy _playerCameraDisplayStrategy;
        private TopCameraDisplayStrategy _topCameraDisplayStrategy;
        private MultiviewDisplayStrategy _multiviewDisplayStrategy;
        private static readonly int SlideDown = Animator.StringToHash("SlideDown");

        private void Awake()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            networkManager.OnClientDisconnectAction += DisplayError;
            _playerCameraDisplayStrategy = new PlayerCameraDisplayStrategy(this);
            _topCameraDisplayStrategy = new TopCameraDisplayStrategy(this);
            _multiviewDisplayStrategy = new MultiviewDisplayStrategy(this);
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
            if (!currentPlayMode.Equals(PlayMode.None))
            {
                SetCurrentPlayMode(currentPlayMode.ToString());
            }

            timer.gameObject.SetActive(portraitOriented);

            controlButtons.transform.localScale = portraitOriented
                ? UIConstants.ControlsLocalScalePortrait
                : UIConstants.ControlsLocalScaleLandscape;
        }

        public void ToggleControlsVisible()
        {
            controlsVisible = !controlsVisible;
            controls.SetActive(controlsVisible);
        }

        public void OnPlayPressed(bool playing)
        {
            playButton.SetSelected(playing);
            timer.SetTimerPlaying(playing);
            progressBar.SetProgressBarPlaying(playing);
        }


        public void OnSceneChosen(string chosenScene)
        {
            mobileController.OnSceneSelected(chosenScene);
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            StartCoroutine(SlideSplashScreen());
        }

        public void OnSceneLoaded()
        {
            endButton.interactable = true;
        }

        public void AddPanelToStack(string panelName)
        {
            _panelOnTopOfStack = panelName;
            Enable(panelName, true);
            Enable(UIConstants.BackButton, true);

            if (!portraitOriented)
            {
                OnChangeScreenOrientation();
            }
        }

        public void RemovePanelFromStack()
        {
            Enable(_panelOnTopOfStack, false);
            Enable(UIConstants.BackButton, false);
        }


        public void SetPlayButtonSelected(bool playing) //TODO
        {
            // playButton.SetSelected(playing);
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
            var speed = speedSlider.value;
            sliderText.text = speed.ToString();
            mobileController.SetSpeed(speed);
            progressBar.SetSpeed(speed);
        }

        public void OnGoToLobby()
        {
            StartCoroutine(ActivateButtons(sceneButtons, 0, false));
            StartCoroutine(ActivateButtons(sceneButtons, GameConstants.ReturnToLobbyWaitingTime, true));

            EnablePanelExclusive(UIConstants.ConnectScreen);
            EnableCameraView(UIConstants.PlayerCameraRT);
            EnableTrue(UIConstants.SceneSelection);
            EnableFalse(UIConstants.VideoControls);
            EnableFalse(UIConstants.SceneJoin);

            timer.ResetTimer();
            currentPlayMode = PlayMode.None;
            portraitOriented = false;
            OnChangeScreenOrientation();

            SetPlayButtonSelected(false);
            progressBar.ResetProgressBar();
            endButton.interactable = false;
        }

        private IEnumerator SlideSplashScreen()
        {
            yield return new WaitForSecondsRealtime(3f);
            splashScreen.GetComponent<Animator>().SetTrigger(SlideDown);
            yield return new WaitForSecondsRealtime(2f);
            splashScreen.SetActive(false);
        }

        private PlayMode ParsePlayMode(string playMode)
        {
            switch (playMode)
            {
                case "PlayerCamera":
                    return PlayMode.PlayerCamera;
                case "TopCamera":
                    return PlayMode.TopCamera;
                default:
                    return PlayMode.Multiview;
            }
        }

        public void SetCurrentPlayMode(string playMode)
        {
            currentPlayMode = ParsePlayMode(playMode);
            mobileController.EnableCamera(currentPlayMode, portraitOriented);
            switch (currentPlayMode)
            {
                case PlayMode.PlayerCamera:
                    _playerCameraDisplayStrategy.DisplaySelf(portraitOriented);
                    break;
                case PlayMode.TopCamera:
                    _topCameraDisplayStrategy.DisplaySelf(portraitOriented);
                    break;
                case PlayMode.Multiview:
                    _multiviewDisplayStrategy.DisplaySelf(portraitOriented);
                    break;
            }
        }
    }
}