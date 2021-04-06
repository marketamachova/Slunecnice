using System.Collections;
using System.Collections.Generic;
using Player;
using UnityEngine;
using UnityEngine.Serialization;
using NetworkPlayer = Network.NetworkPlayer;

namespace UI
{
    public class UIControllerMobile : BaseUIController
    {
        [Header("Runtime")]
        [SerializeField] public bool portraitOriented;
        [SerializeField] private bool playing = false;
        [SerializeField] private bool controlsVisible;


        [Header("Mobile UI Elements")]
        [SerializeField] private List<GameObject> cameraViews;
        [SerializeField] private GameObject controls;
        [SerializeField] private Timer timer;
        [SerializeField] private ProgressBar progressBar;

        [Header("Special Buttons")]
        [SerializeField] private Selectable playButton;
        [SerializeField] private Selectable maximizeButton;

        [FormerlySerializedAs("controller")] [SerializeField] private MobileController mobileController;
        
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
            this.playing = playing;
            timer.SetTimerPlaying(playing);
            progressBar.SetProgressBarPlaying(playing);
        }
        
        

        //general controller
        public void OnSceneChosen(string chosenScene)
        {
            mobileController.OnSceneSelected(chosenScene);
            timer.ResetTimer(); //nevim
        }
    }
}