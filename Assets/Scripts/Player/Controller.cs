using Network;
using UI;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Player
{
    public class Controller : BaseController
    {
        [SerializeField] private UIControllerMobile uiControllerMobile;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private MyNetworkManager networkManager;
        private bool _playing = false;

        public void Awake()
        {
            sceneLoader.SceneLoadingEnd += OnSceneLoaded;
            networkManager.OnServerAddPlayerAction += AssignPlayers;
        }
        
        private void OnSceneLoaded()
        {
            Debug.Log("Controller scene loaded");
            uiControllerMobile.EnablePanelExclusive("WatchScreenPortrait");
            uiControllerMobile.ToggleControlsVisible();
            uiControllerMobile.ActivateExclusive("PlayerCameraButton");
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

        public void PauseDrive()
        {
            Debug.Log("Mobile Controller pause drive");
            LocalNetworkPlayer.CmdSetPlayerMoving(false);
        }

        public void ResumeDrive()
        {
            Debug.Log("Mobile Controller resume drive");
            LocalNetworkPlayer.CmdSetPlayerMoving(true);
        }

        public void EndDrive()
        {
            Debug.Log("Mobile Controller end drive");
            LocalNetworkPlayer.CmdSetPlayerMoving(false); //indicate END somehow
        }

        public void EndCalibration() //TODO
        {
            Debug.Log("Mobile Controller end calibration");
            // LocalNetworkPlayer.CmdSetCalibration(false); //TODO
        }

        public override void AssignPlayers()
        {
            base.AssignPlayers();
            Debug.Log("Controller assign playersss");
            Debug.Log(LocalNetworkPlayer.netId);
            LocalNetworkPlayer.OnCalibrationComplete += OnCalibrationComplete;
        }

        protected override void OnCalibrationComplete()
        {
            Debug.Log("HIDE CALIBRATION MOBILE CONTROLLER");
            Debug.Log(uiControllerMobile);
            uiControllerMobile.EnableFalse("Calibration");
            uiControllerMobile.EnableTrue("SceneSelection");
        }
    }
}