using System.Collections;
using Cart;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
    public enum World
    {
        MainScene,
        WinterScene,
        RuralScene
    }

    public class VRLobbyController : BaseController
    {
        [SerializeField] private CartCreator cartCreator;

        private bool _debug = true;

        private void Awake()
        {
            sceneLoader.SceneLoadingEnd += OnSceneLoaded;
            networkManager.OnClientConnectAction += OnClientConnected;
            networkManager.OnMobileClientConnectAction += OnClientMobileConnected;
            networkManager.OnClientDisconnectAction += OnClientDisonnected;
            networkManager.OnMobileClientDisconnectAction += OnClientMobileDisconnected;
            cartCreator.OnCartCreatorCalibrationComplete += SetCalibrationComplete;
        }

        private IEnumerator Start()
        {
            yield return new WaitForSecondsRealtime(1);
            NetworkManager.singleton.StartHost();
            networkDiscovery.AdvertiseServer();
        }

        //debug only
        private void Update()
        {
            if (_debug)
            {
                if (Input.GetKey(KeyCode.R))
                {
                    OnSceneSelected(World.RuralScene.ToString());
                }
                if (Input.GetKey(KeyCode.W))
                {
                    OnSceneSelected(World.WinterScene.ToString());
                }
                if (Input.GetKey(KeyCode.M))
                {
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
            LocalNetworkPlayer.CmdSetPlayerMoving(true);
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

        public override void SkipCalibration()
        {
            base.SkipCalibration();
            cartCreator.SkipCalibration();
        }

        public override void OnSceneLoaded()
        {
            uiController.gameObject.SetActive(false);

            Debug.Log("LocalNetworkPlayer.chosenWorld " + LocalNetworkPlayer.chosenWorld);
            var scene = SceneManager.GetSceneByName(LocalNetworkPlayer.chosenWorld);
            SceneManager.SetActiveScene(scene);
        }

        public override void OnGoToLobby()
        {
            Debug.Log("on go to lobbyyyyyyyy");
            uiController.gameObject.SetActive(true);
            LocalNetworkPlayer.CmdSetWorldLoaded(false);
        }
        
        public CartCreator GetCartCreator()
        {
            return cartCreator;
        }

       
    }
}