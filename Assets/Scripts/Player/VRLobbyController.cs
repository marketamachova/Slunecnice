using System;
using System.Collections;
using Cart;
using Mirror;
using Mirror.Discovery;
using Network;
using UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngineInternal;
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
            uiController.DisplayLoader();
            sceneLoader.LoadScene(scene, false);

            // text.GetComponent<Text>().text = "Players count: " + networkManager.Players.Count;
            // networkManager.Players.ForEach(player 
            //     => player.GetComponent<NetworkPlayer>().LoadChosenScene(networkManager.Players, scene));
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
            uiController.Deactivate("AvailabilityIndicatorMobile");
        }

        private void OnCalibrationCompleteNetwork()
        {
            var player = GameObject.FindWithTag("Player").GetComponent<NetworkPlayer>();
            player.calibrationComplete = true;
        }
    }
}