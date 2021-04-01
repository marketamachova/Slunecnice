using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using NetworkPlayer = Network.NetworkPlayer;

namespace UI
{
    enum WatchMode {PlayerCamera, TopCamera, Dashboard}

    public class UIControllerMobile : BaseUIController
    {
        [SerializeField] public bool portraitOriented;
        
        [SerializeField] private List<GameObject> screenPanels;
        [SerializeField] private List<Selectable> buttons;
        [SerializeField] private List<GameObject> cameraViews;
        [SerializeField] private GameObject controls;
        [SerializeField] private GameObject sceneSelectionPanel;
        [SerializeField] private Timer timer;
        [SerializeField] private ProgressBar progressBar;
        [SerializeField] private Selectable playButton;
        [SerializeField] private Selectable maximizeButton;
        
        private NetworkPlayer _networkPlayer;
        private SceneLoader _sceneLoader;
        private bool _controlsVisible;
        private bool _playing = false;

        private void Awake()
        {
            networkManager.OnClientDisconnectAction += DisplayError;
            _sceneLoader = GetComponent<SceneLoader>();
        }

        // public void EnablePanel(string panelName)
        // {
        //     screenPanels.ForEach(panel => panel.SetActive(panel.name == panelName));
        // }

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
            _controlsVisible = !_controlsVisible;
            controls.SetActive(_controlsVisible);
        }

        private IEnumerator HideControls()
        {
            while (_controlsVisible)
            {
                yield return new WaitForSecondsRealtime(5);
                controls.SetActive(false);
            }
        }

        public void OnPlayPressed(bool playing)
        {
            playButton.SetSelected(playing);
            _playing = playing;
            timer.SetTimerPlaying(playing);
            progressBar.SetProgressBarPlaying(playing);
        }

        //general controller
        public void OnSceneChosen(string chosenScene)
        {
            _networkPlayer.CmdHandleSelectedWorld(chosenScene); //message about scene loading to other players
            _sceneLoader.LoadScene(chosenScene, true);
            timer.ResetTimer(); //nevim
        }


        //general controller
        public void AssignPlayer(NetworkPlayer player)
        {
            Debug.Log("assigning player ");
            _networkPlayer = player;
            Debug.Log(_networkPlayer);
        }
    }
}