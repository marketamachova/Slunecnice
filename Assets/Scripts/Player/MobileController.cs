using System.Linq;
using Scenes;
using UI;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;
using PlayMode = UI.PlayMode;

namespace Player
{
    public class MobileController : BaseController
    {
        [SerializeField] private UIControllerMobile uiControllerMobile;
        [SerializeField] private ConnectScreenController connectController;
        
        private bool _playing = false;
        private SceneLoader _sceneLoader;
        private NetworkPlayer _vrPlayer;
        private GameObject[] _cameras;

        public void Awake()
        {
            _sceneLoader = GetComponent<SceneLoader>();

            _sceneLoader.SceneLoadingEnd += OnSceneLoaded;
            networkManager.OnServerAddPlayerAction += AssignPlayers;

            SetLanguage();

            AssignPlayers();
        }

        /**
         * mobile client should wait for the moment a world is loaded in the VR application
         * - called on mobile application scene load finish
         * - if VR app has scene already loaded, mobile proceeds to display relevant UI
         * - if not, an event is subscribed
         */
        protected override void OnSceneLoaded()
        {
            base.OnSceneLoaded();

            if (RemoteNetworkPlayer.worldLoaded)
            {
                uiControllerMobile.EnablePanelExclusive(UIConstants.WatchScreenPortrait);
                uiControllerMobile.EnableTrue(UIConstants.VideoControls);
                uiControllerMobile.OnSceneLoaded();

                _playing = RemoteNetworkPlayer.playerMoving;

                foreach (var networkPlayer in NetworkPlayers)
                {
                    networkPlayer.CmdTriggerTimeSync(true);
                }

                LocalNetworkPlayer.CmdSetPlayerMoving(_playing);
                uiControllerMobile.SetPlayButtonSelected(_playing);
                EnableCamera(PlayMode.PlayerCamera, true);
            }

            else
            {
                RemoteNetworkPlayer.OnSceneLoadedAction += OnSceneLoaded;
            }
        }

        public override void OnDisconnect()
        {
            base.OnDisconnect();
            uiControllerMobile.EnablePanelExclusive(UIConstants.ConnectScreen);
            connectController.OnDisconnect();
            sceneLoader.UnloadScene();
        }

        /**
         * handles pressing the Play button
         */
        public void OnPlayPressed()
        {
            _playing = !LocalNetworkPlayer.playerMoving;

            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }

            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSetPlayerMoving(_playing);
            }

            uiControllerMobile.OnPlayPressed(_playing);
        }
        

        public override void SkipCalibration()
        {
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSkipCalibration(true);
            }
        }

        /**
         * called when mobile app connects the VR app
         * handles the situations, when:
         * - VR application is in calibration process
         * - VR application is in scene selection process
         * - VR application has scene selected
         */
        public void AssignPlayer(NetworkPlayer networkPlayer)
        {
            LocalNetworkPlayer = networkPlayer;
            _vrPlayer = networkPlayer;
            foreach (var player in FindObjectsOfType<NetworkPlayer>().Where(p => !p.isLocalPlayer && !p.mobile))
            {
                _vrPlayer = player;
            }

            Debug.Log("_vrPlayer.mobile " + _vrPlayer.mobile);
            // if (_vrPlayer.calibrationComplete)
            // {
            //     LocalNetworkPlayer.CmdSetCalibrationComplete(true);
            // }
            if (_vrPlayer.playerMoving)
            {
                LocalNetworkPlayer.CmdSetPlayerMoving(true);
            }
            
            if (!string.IsNullOrEmpty(_vrPlayer.chosenWorld)) //scene selected in VR
            {
                DisplaySceneSelected(_vrPlayer.chosenWorld);
            }
            else if (_vrPlayer.calibrationComplete) //calibration complete in VR
            {
                LocalNetworkPlayer.CmdSetCalibrationComplete(true);
                OnCalibrationComplete();
            }
            else //calibration in process
            {
                uiControllerMobile.EnableTrue(UIConstants.Calibration); // display "Calibration in process message"
                LocalNetworkPlayer.OnCalibrationComplete +=
                    OnCalibrationComplete; //observe calibration complete process 
            }

            AssignCameras();
        }

        protected override void OnCalibrationComplete()
        {
            base.OnCalibrationComplete();
            if (!string.IsNullOrEmpty(_vrPlayer.chosenWorld))
            {
                DisplaySceneSelected(_vrPlayer.chosenWorld);
            }
            else
            {
                uiController.EnableTrue(UIConstants.SceneSelection);
            }

            uiController.EnableFalse(UIConstants.Calibration);
        }

        public override void OnGoToLobby()
        {
            base.OnGoToLobby();
            sceneLoader.UnloadScene();
            uiControllerMobile.OnGoToLobby();
        }

        private void DisplaySceneSelected(string sceneName)
        {
            uiController.EnableFalse(UIConstants.SceneSelection);
            uiController.EnableTrue(UIConstants.SceneJoin);
        }

        public void OnLoadedSceneJoin()
        {
            OnSceneSelected(_vrPlayer.chosenWorld);
        }

        public void SetSpeed(float value)
        {
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSetSpeed((int) value);
            }
        }

        public void EnableCamera(PlayMode playMode, bool portrait)
        {
            switch (playMode)
            {
                case PlayMode.PlayerCamera:
                    EnableCamerasExclusive(new []{portrait ? UIConstants.RTCamera : UIConstants.RTCameraLandscape, ""});
                    break;
                case PlayMode.TopCamera:
                    EnableCamerasExclusive(new []{portrait ? UIConstants.RTTopCamera : UIConstants.RTTopCameraLandscape, ""});
                    break;
                case PlayMode.Multiview:
                    EnableCamerasExclusive(new []{UIConstants.RTCamera, UIConstants.RTTopCamera});
                    break;
            }
        }

        private void EnableCamerasExclusive(string[] activeCameras)
        {
            foreach (var o in _cameras)
            {
                o.GetComponent<Camera>().enabled = o.name.Equals(activeCameras[0]) || o.name.Equals(activeCameras[1]);
            }
        }

        private void AssignCameras()
        {
            _cameras = GameObject.FindGameObjectsWithTag(UIConstants.RTCamera);
        }

        private void SetLanguage()
        {
            var dropdownValueIndex = Application.systemLanguage == SystemLanguage.Czech ? 0 : 1;
            uiController.SetLanguageDropdownValue(dropdownValueIndex);
        }
    }
}