using System.Collections;
using Cart;
using Mirror;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Player
{
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

        protected override void OnCalibrationComplete()
        {
            base.OnCalibrationComplete();
            uiController.EnablePanelExclusive(UIConstants.SceneSelection);
        }

        public override void SkipCalibration()
        {
            base.SkipCalibration();
            cartCreator.SkipCalibration();
        }

        protected override void OnSceneLoaded()
        {
            uiController.gameObject.SetActive(false);

            var scene = SceneManager.GetSceneByName(LocalNetworkPlayer.chosenWorld);
            SceneManager.SetActiveScene(scene);
        }

        public override void OnGoToLobby()
        {
            uiController.gameObject.SetActive(true);
            LocalNetworkPlayer.CmdSetWorldLoaded(false);
        }
    }
}