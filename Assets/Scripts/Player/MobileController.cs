using System.Linq;
using Scenes;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
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

        public override void OnSceneLoaded()
        {
            base.OnSceneLoaded();
            Debug.Log("ON SCENE LOADED");

            // var scene = SceneManager.GetSceneByName(LocalNetworkPlayer.chosenWorld + "Mobile");
            var scene = SceneManager.GetSceneByName("EWinterMobile");
            SceneManager.SetActiveScene(scene);

            // GameObject.FindObjectOfType<Canvas>().gameObject.SetActive(false);

            if (RemoteNetworkPlayer.worldLoaded)
            {
                Debug.Log("RemoteNetworkPlayer.worldLoaded");

                uiControllerMobile.EnablePanelExclusive("WatchScreenPortrait");
                uiControllerMobile.EnableTrue("VideoControls");

                _playing = RemoteNetworkPlayer.playerMoving;

                // if (NetworkPlayers.Length < 2)
                // {
                //     AssignPlayers();
                // }

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

            Debug.Log("MobileController.OnDisconnect");
        }

        public void OnPlayPressed()
        {
            Debug.Log("playing " + LocalNetworkPlayer.playerMoving);

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

        // public void EndDrive()
        // {
        //     Debug.Log("Mobile Controller end drive");
        //     LocalNetworkPlayer.CmdSetPlayerMoving(false);
        //     
        // }
        
        

        public override void SkipCalibration()
        {
            // if (NetworkPlayers.Length < 2)
            // {
            //     AssignPlayers();
            // }

            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdSkipCalibration(true);
            }
        }

        public void AssignPlayer(NetworkPlayer networkPlayer)
        {
            LocalNetworkPlayer = networkPlayer;
            _vrPlayer = networkPlayer;
            foreach (var player in FindObjectsOfType<NetworkPlayer>().Where(p => !p.isLocalPlayer))
            {
                _vrPlayer = player;
            }

            if (_vrPlayer.calibrationComplete)
            {
                LocalNetworkPlayer.CmdSetCalibrationComplete(true);
            }

            if (_vrPlayer.playerMoving)
            {
                LocalNetworkPlayer.CmdSetPlayerMoving(true);
            }
            
            Debug.Log("ASSIGN PLAYER " + networkPlayer);
            
            if (_vrPlayer.calibrationComplete) //calibration complete in VR
            {
                Debug.Log("calibration complete");
                OnCalibrationComplete();
            }
            else //calibration in process
            {
                Debug.Log("calibration in processs.......");
                uiControllerMobile.EnableTrue("Calibration"); // display "Calibration in process message"
                LocalNetworkPlayer.OnCalibrationComplete +=
                    OnCalibrationComplete; //observe calibration complete process 
            }

            if (!string.IsNullOrEmpty(_vrPlayer.chosenWorld)) //scene selected in VR
            {
                Debug.Log("chosen world " + _vrPlayer.chosenWorld);
                DisplaySceneSelected(_vrPlayer.chosenWorld);
            }

            AssignCameras();
        }

        public override void OnCalibrationComplete()
        {
            base.OnCalibrationComplete();
            Debug.Log("HIDE CALIBRATION MOBILE CONTROLLER");
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
            Debug.Log("display scene selected");
            uiController.EnableFalse(UIConstants.SceneSelection);
            uiController.EnableTrue(UIConstants.SceneJoin);
        }

        public void OnLoadedSceneJoin()
        {
            OnSceneSelected(_vrPlayer.chosenWorld);
        }

        public void SetSpeed(float value)
        {
            Debug.Log("setting speed to " + value);
            
            // AssignPlayers();
            
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