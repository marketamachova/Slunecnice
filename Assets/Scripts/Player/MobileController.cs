using Network;
using UI;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    public class MobileController : BaseController
    {
        [SerializeField] private UIControllerMobile uiControllerMobile;
        [SerializeField] private ConnectScreenController connectController;
        private bool _playing = true;
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
            // uiControllerMobile.ActivateExclusive("PlayerCameraButton");
        }

        public override void OnDisconnect()
        {
            base.OnDisconnect();
            uiControllerMobile.EnablePanelExclusive("ConnectScreen");
            Debug.Log("ON DISCONNECT Controller");
        }

        public void OnPlayPressed()
        {
            Debug.Log("play pressed NOW");

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

        public void SkipCalibration() //TODO
        {
            Debug.Log("Mobile Controller end calibration");
            LocalNetworkPlayer.CmdSkipCalibration(true); //TODO
        }

        public void AssignPlayer(NetworkPlayer networkPlayer)
        {
            LocalNetworkPlayer = networkPlayer;
            if (LocalNetworkPlayer.calibrationComplete) //calibration complete in VR
            {
                //display scene selection
            }
            else if (!string.IsNullOrEmpty(LocalNetworkPlayer.chosenWorld)) //scene selected in VR
            {
                //load chosen world and get VR player's position
            }
            else //calibration in process
            {
                Debug.Log("calibration in processs.......");
                uiControllerMobile.EnableTrue("Calibration"); // display "Calibration in process message"
                LocalNetworkPlayer.OnCalibrationComplete +=
                    OnCalibrationComplete; //observe calibration complete process TODO tohle mozna nebude potreba
            }
        }

        public override void OnCalibrationComplete()
        {
            base.OnCalibrationComplete();
            Debug.Log("HIDE CALIBRATION MOBILE CONTROLLER");
            uiControllerMobile.EnableFalse("Calibration");
            uiControllerMobile.EnableTrue("SceneSelection");
        }
    }
}