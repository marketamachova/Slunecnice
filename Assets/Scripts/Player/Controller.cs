using Network;
using UI;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    public class Controller : BaseController
    {
        [SerializeField] private UIControllerMobile uiControllerMobile;
        [SerializeField] private MyNetworkManager networkManager;
        [SerializeField] private ConnectScreenController connectController;
        private bool _playing = false;
        private SceneLoader _sceneLoader;

        public void Awake()
        {
            _sceneLoader = GetComponent<SceneLoader>();
            Debug.Log(_sceneLoader);
            _sceneLoader.SceneLoadingEnd += OnSceneLoaded;
            networkManager.OnServerAddPlayerAction += AssignPlayers;
        }
        
        private void OnSceneLoaded()
        {
            Debug.Log("Controller scene loaded");
            uiControllerMobile.EnablePanelExclusive("WatchScreenPortrait");
            uiControllerMobile.ToggleControlsVisible();
            uiControllerMobile.ActivateExclusive("PlayerCameraButton");
        }

        public void HandleSceneChosen(string sceneName)
        {
            LocalNetworkPlayer.CmdHandleSelectedWorld(sceneName); //message about scene loading to other players
            _sceneLoader.LoadScene(sceneName, true);
        }

        public void OnDisconnect()
        {
            uiControllerMobile.EnablePanelExclusive("ConnectScreen");
            Debug.Log("ON DISCONNECT Controller");
        }
        
        public void OnPlayPressed()
        {
            _playing = !_playing;
        
            if (Players.Count == 0)
            {
                AssignPlayers();
            }

            foreach (var networkPlayer in Players)
            {
                Debug.Log(networkPlayer);
                if (networkPlayer.hasAuthority)
                {
                    networkPlayer.CmdSetPlayerMoving(_playing);
                }
            }
        
            uiControllerMobile.OnPlayPressed(_playing);
        }

        public void EndDrive()
        {
            Debug.Log("Mobile Controller end drive");
            LocalNetworkPlayer.CmdSetPlayerMoving(false); //indicate END somehow
        }

        public void EndCalibration() //TODO
        {
            Debug.Log("Mobile Controller end calibration");
            LocalNetworkPlayer.CmdSkipCalibration(true); //TODO
        }

        // public override void AssignPlayers()
        // {
        //     base.AssignPlayers();
        //     Debug.Log("Controller assign playersss");
        //     Debug.Log(LocalNetworkPlayer.netId);
        //     Debug.Log(uiControllerMobile);
        //     uiControllerMobile.EnableFalse("Calibration");
        //     uiControllerMobile.EnableTrue("SceneSelection");
        //     //LocalNetworkPlayer.OnCalibrationComplete += OnCalibrationComplete; TODO
        // }

        public void AssignPlayer(NetworkPlayer networkPlayer)
        {
            LocalNetworkPlayer = networkPlayer;
            if (LocalNetworkPlayer.calibrationComplete) //calibration complete in VR
            {
                //display scene selection
            } else if (!string.IsNullOrEmpty(LocalNetworkPlayer.chosenWorld)) //scene selected in VR
            {
                //load chosen world and get VR player's position
            }
            else //calibration in process
            {
                Debug.Log("calibration in processs.......");
                uiControllerMobile.EnableTrue("Calibration"); // display "Calibration in process message"
                LocalNetworkPlayer.OnCalibrationComplete += OnCalibrationComplete; //observe calibration complete process
            }
        }

        public override void OnCalibrationComplete()
        {
            Debug.Log("HIDE CALIBRATION MOBILE CONTROLLER");
            Debug.Log(uiControllerMobile);
            uiControllerMobile.EnableFalse("Calibration");
            uiControllerMobile.EnableTrue("SceneSelection");
        }
    }
}