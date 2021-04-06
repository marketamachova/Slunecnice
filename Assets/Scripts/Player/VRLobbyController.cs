using System;
using System.Collections;
using Cart;
using Mirror;
using Mirror.Discovery;
using Network;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using NetworkPlayer = Network.NetworkPlayer;

namespace Player
{
    public enum World
    {
        MainScene,
        Forest,
        Winter,
        Spring
    }

    public class VRLobbyController : NetworkBehaviour
    {
        [SerializeField] private NetworkDiscovery networkDiscovery;
        [SerializeField] private MyNetworkManager networkManager;
        [SerializeField] private UIControllerVRLobby uiController;
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private CartCreator cartCreator;

        //debug only
        private bool _spacePressed = false;

        private void Awake()
        {
            networkManager.OnClientConnectAction += OnClientConnected;
            networkManager.OnMobileClientConnectAction += OnClientMobileConnected;
            networkManager.OnClientDisconnectAction += OnClientDisonnected;
            networkManager.OnMobileClientDisconnectAction += OnClientMobileDisonnected;
            cartCreator.OnCalibrationComplete += OnCalibrationCompleteNetwork;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            NetworkManager.singleton.StartHost(); //todo to myNetowrk manager STATIC
            networkDiscovery.AdvertiseServer();
        }

        private void Update()
        {
            //debug only
            if (Input.GetKey(KeyCode.Space) && !_spacePressed)
            {
                _spacePressed = true;
                OnSceneSelected(World.MainScene.ToString());
            }
        }

        public void OnSceneSelected(string scene)
        {
            if (SceneManager.sceneCount > 1)
            {
                return;
            }
            uiController.DisplayLoader(true);
            sceneLoader.LoadScene(scene, true);
        }

        private void OnClientConnected()
        {
            uiController.Activate("AvailabilityIndicatorVR");
        }

        private void OnClientMobileConnected()
        {
            uiController.Activate("AvailabilityIndicatorMobile");
        }

        private void OnClientDisonnected()
        {
            uiController.Deactivate("AvailabilityIndicatorVR");
        }

        private void OnClientMobileDisonnected()
        {
            Debug.Log("OMCLIENTMOBILEDISCONNECT");
            uiController.Deactivate("AvailabilityIndicatorMobile");
        }

        private void OnCalibrationCompleteNetwork()
        {
            Debug.Log("Oncalibration complete network vRLOBBY CONTROLLER");
            uiController.EnablePanelExclusive("SceneSelection");
            var players = GameObject.FindObjectsOfType<NetworkPlayer>();
            foreach (var networkPlayer in players)
            {
                networkPlayer.CmdSetCalibrationComplete(true);
            }
        }

        public CartCreator GetCartCreator()
        {
            return cartCreator;
        }
    }
}