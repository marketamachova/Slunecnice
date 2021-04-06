using System;
using System.Collections;
using Cart;
using Mirror;
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

    public class VRLobbyController : BaseController
    {
        [SerializeField] private CartCreator cartCreator;

        [Header("Debug only")] private bool _spacePressed = false;
        private bool _debug = true;

        private void Awake()
        {
            networkManager.OnClientConnectAction += OnClientConnected;
            networkManager.OnMobileClientConnectAction += OnClientMobileConnected;
            networkManager.OnClientDisconnectAction += OnClientDisonnected;
            networkManager.OnMobileClientDisconnectAction += OnClientMobileDisconnected;
            cartCreator.OnCartCreatorCalibrationComplete += SetCalibrationComplete;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            NetworkManager.singleton.StartHost(); //todo to myNetowrk manager STATIC
            networkDiscovery.AdvertiseServer();
        }

        //debug only
        private void Update()
        {
            if (_debug)
            {
                if (Input.GetKey(KeyCode.Space) && !_spacePressed)
                {
                    _spacePressed = true;
                    OnSceneSelected(World.MainScene.ToString());
                }
            }
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

        private void OnClientMobileDisconnected()
        {
            uiController.Deactivate("AvailabilityIndicatorMobile");
        }

        public override void SetCalibrationComplete()
        {
            base.SetCalibrationComplete();
            Debug.Log("SET calibration complete network vRLOBBY CONTROLLER");
        }

        public override void OnCalibrationComplete()
        {
            base.OnCalibrationComplete();
            uiController.EnablePanelExclusive("SceneSelection");
            Debug.Log("ON calibration complete network vRLOBBY CONTROLLER");
        }

        public CartCreator GetCartCreator()
        {
            return cartCreator;
        }
    }
}