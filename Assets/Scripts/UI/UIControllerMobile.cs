using System.Collections;
using System.Collections.Generic;
using Player;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

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

        [Header("Mobile UI Elements")] [SerializeField]
        private List<GameObject> cameraViews;

        [SerializeField] private GameObject controls;
        [SerializeField] private GameObject controlButtons;
        [SerializeField] private Timer timer;
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private Button[] sceneButtons;

        [Header("Special Elements")] [SerializeField]
        private Selectable playButton;

        [SerializeField] private Selectable maximizeButton;
        [SerializeField] private MobileController mobileController;
        [SerializeField] private Slider speedSlider;
        [SerializeField] private TextMeshProUGUI sliderText;
        [SerializeField] private Button endButton;

        private string _panelOnTopOfStack;
        private PlayerCameraDisplayStrategy _playerCameraDisplayStrategy;
        private TopCameraDisplayStrategy _topCameraDisplayStrategy;
        private MultiviewDisplayStrategy _multiviewDisplayStrategy;

        private void Awake()
        {
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
            Debug.Log("ONchange Screen orientation");
            Screen.orientation = portraitOriented ? ScreenOrientation.LandscapeLeft : ScreenOrientation.Portrait;
            portraitOriented = !portraitOriented;
            maximizeButton.SetSelected(!portraitOriented);
            if (!currentPlayMode.Equals(PlayMode.None))
            {
                SetCurrentPlayMode(currentPlayMode.ToString());
            }

            if (!portraitOriented)
            {
                controlButtons.transform.localScale = new Vector3(1.559694f, 0.1559289f, 1.559694f);
                timer.transform.localScale = new Vector3(1.559694f, 0.1559289f, 1.559694f);
            }
            else
            {
                controlButtons.transform.localScale = new Vector3(1.034554f, 0.1034285f, 1.034554f);
                timer.transform.localScale = new Vector3(1.034554f, 1.034554f, 1.034554f);
            }
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
            Debug.Log("ON PLAY PRESSED MOBLE CONTROLEL playing " + playing);
            playButton.SetSelected(playing);
            timer.SetTimerPlaying(playing);
            progressBar.SetProgressBarPlaying(playing);
        }


        //general controller
        public void OnSceneChosen(string chosenScene)
        {
            mobileController.OnSceneSelected(chosenScene);
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
            Debug.Log("on go to lobby");
            StartCoroutine(ActivateButtons(sceneButtons, 0, false));
            StartCoroutine(ActivateButtons(sceneButtons, 3, true));
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