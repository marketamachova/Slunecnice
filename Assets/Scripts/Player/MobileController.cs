﻿using System.Linq;
using Network;
using Scenes;
using UI;
using UnityEngine;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    public class MobileController : BaseController
    {
        [SerializeField] private UIControllerMobile uiControllerMobile;
        [SerializeField] private ConnectScreenController connectController;
        private bool _playing = false;
        private SceneLoader _sceneLoader;
        private NetworkPlayer _vrPlayer;

        public void Awake()
        {
            _sceneLoader = GetComponent<SceneLoader>();

            _sceneLoader.SceneLoadingEnd += OnSceneLoaded;
            networkManager.OnServerAddPlayerAction += AssignPlayers;

            AssignPlayers();
        }

        private void OnSceneLoaded()
        {
            uiControllerMobile.EnablePanelExclusive("WatchScreenPortrait");
            uiControllerMobile.EnableTrue("VideoControls");

            Debug.Log("RemoteNetworkPlayer.playerMoving " + RemoteNetworkPlayer.playerMoving);
            _playing = RemoteNetworkPlayer.playerMoving;

            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }
            foreach (var networkPlayer in NetworkPlayers)
            {
                networkPlayer.CmdTriggerTimeSync();
            }


            LocalNetworkPlayer.CmdSetPlayerMoving(_playing);
            uiControllerMobile.SetPlayButtonSelected(_playing);
        }


        public override void OnDisconnect()
        {
            base.OnDisconnect();
            uiControllerMobile.EnablePanelExclusive("ConnectScreen");
            connectController.OnDisconnect();

            Debug.Log("ON DISCONNECT Controller");
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

        public void EndDrive()
        {
            Debug.Log("Mobile Controller end drive");
            LocalNetworkPlayer.CmdSetPlayerMoving(false); //indicate END somehow
        }

        public void SkipCalibration()
        {
            if (NetworkPlayers.Length < 2)
            {
                AssignPlayers();
            }

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
                uiController.EnableTrue("SceneSelection");

            }
            uiController.EnableFalse("Calibration");
        }

        public override void OnGoToLobby()
        {
            base.OnGoToLobby();
            sceneLoader.UnloadScene();
            uiController.EnablePanelExclusive("ConnectScreen");
            uiController.EnableTrue("SceneSelection");
            uiController.EnableFalse("VideoControls");
            uiController.EnableFalse("SceneJoin");
        }

        private void DisplaySceneSelected(string sceneName)
        {
            Debug.Log("display scene selected");
            uiController.EnableFalse("SceneSelection");
            uiController.EnableTrue("SceneJoin");
        }

        public void OnLoadedSceneJoin()
        {
            OnSceneSelected(_vrPlayer.chosenWorld);
        }

        public void SetSpeed(int value)
        {
            Debug.Log("setting speed to " + value);
            AssignPlayers();
            foreach (var networkPlayer in NetworkPlayers) //tady asi neni VR player
            {
                networkPlayer.CmdSetSpeed(value);
            }
        }
    }
}